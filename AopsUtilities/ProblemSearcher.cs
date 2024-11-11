using System.Web;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using DatabaseConnector.Models;

namespace AopsUtilities;

public class ProblemSearcher
{
    private const string SourceUrl = "https://ggim.me/problem/";
    
    public Problem GetFromAops(string problemSource, IProblemIdGenerator problemIdGenerator)
    {
        var document = DownloadProblemHtmlDocument(problemSource);
        if(document == null)
            throw new Exception($"Problem searching for {problemSource} was not found.");
        
        string statement = GetProblemStatement(document);
        string aopsLink = GetProblemLink(document);
        
        ProblemSource source = new ProblemSource(problemSource, aopsLink);
        Description description = new Description(statement);
        
        return new Problem(
            problemIdGenerator.GenerateFromStatement(problemSource, statement),
            source,
            new DescriptionCollection([description]),
            new Difficulty(1,1));
    }
    
    private HtmlDocument? DownloadProblemHtmlDocument(string problemSource)
    {
        var web = new HtmlWeb();
        var document = web.Load(SourceUrl+problemSource);
        return document;
    }
    
    private string GetProblemStatement(HtmlDocument document)
    {
        var node = document.DocumentNode.QuerySelector("code");
        if(node == null)
            throw new Exception($"The webpage scanned does not correspond to the expectations.");
        return HttpUtility.HtmlDecode(node.InnerHtml);
    }
    
    private string GetProblemLink(HtmlDocument document)
    {
        var node = document.DocumentNode.QuerySelectorAll("a");
        if(node.Count < 3)
            throw new Exception($"The webpage scanned does not correspond to the expectations.");
        return node[2].GetAttributeValue("href", "");
    }
    
    
}