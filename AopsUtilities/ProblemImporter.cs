using System.Text;
using System.Web;
using HtmlAgilityPack;
using DatabaseConnector.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace AopsUtilities;

public class ProblemImporter
{
    private readonly IWebDriver _driver;
    private bool _active = true;
    public ProblemImporter(bool headless)
    {
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;
        service.SuppressInitialDiagnosticInformation = true; 
        
        var options = new ChromeOptions();
        if(headless)
            options.AddArguments("headless", "--silent", "log-level=3");
        
        _driver = new ChromeDriver(service, options);
    }

    public void StopDriver()
    {
        _active = false;
        _driver.Quit();
    }
    
    public Problem ScrapProblemFromAops(string url, IProblemIdGenerator problemIdGenerator)
    {
        if(!_active) throw new ApplicationException("The importer have been stopped before the execution of the function.");
        
        _driver.Navigate().GoToUrl(url);

        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElement(By.CssSelector("div.cmty-topic-subject")));

        IWebElement element = _driver.FindElement(By.CssSelector("div.cmty-topic-subject"));
        
        ProblemSource source = new ProblemSource(element.Text, url);
        element = _driver.FindElement(By.CssSelector("div.cmty-post-html"));
        Description description = new Description(ReplaceImagesWithAltTextAndRemoveOthers(element.GetAttribute("innerHTML")));
        
        return new Problem(
            problemIdGenerator.GenerateFromStatement(source.Name, description.Content),
            source,
            new DescriptionCollection([description]),
            new Difficulty(1,1));

    }
    
    private static string ReplaceImagesWithAltTextAndRemoveOthers(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        
        var output = new StringBuilder();
        TurnHtmlNodeIntoPlainText(doc.DocumentNode, output);
        
        return HttpUtility.HtmlDecode(output.ToString());
    }

    private static void TurnHtmlNodeIntoPlainText(HtmlNode node, StringBuilder output)
    {
        foreach (var child in node.ChildNodes)
        {
            if (child.Name == "img")
            {
                var altText = child.GetAttributeValue("alt", string.Empty);
                output.Append(altText);
            }
            else if (child.NodeType == HtmlNodeType.Text)
            {
                output.Append(child.InnerText);
            }
            else
            {
                TurnHtmlNodeIntoPlainText(child, output);
            }
        }
    }
    
}