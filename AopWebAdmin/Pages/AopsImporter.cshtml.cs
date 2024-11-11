using AopsUtilities;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AopWebAdmin.Pages;

public class AopsImporter : PageModel
{
    private static ProblemSearcher _problemSearcher;
    private static ProblemImporter _problemScraper;

    public static void Init()
    {
        _problemSearcher = new();
        _problemScraper = new(true);
    }
    public Problem? Problem { get; set; }

    public bool DidntFindProblem { get; set; } = false;
    
    public ProblemSearcher ProblemSearcher { get{return _problemSearcher;} }
    
    [BindProperty]
    public string Search { get; set; } = string.Empty;
    
    private readonly ILogger<ErrorModel> _logger;

    public AopsImporter(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }


    public void OnPost()
    {
        if (!String.IsNullOrEmpty(Search))
        {
            try
            {
                if(Search.Contains("http"))
                {
                    Problem = _problemScraper.ScrapProblemFromAops(Search, new HashGenerator());
                }
                else
                {
                    Problem = _problemSearcher.GetFromAops(Search, new HashGenerator());
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                DidntFindProblem = true;
            }
        }
    }
}