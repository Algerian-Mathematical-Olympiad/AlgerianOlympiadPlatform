using AopsUtilities;
using DatabaseConnector;
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
        //_problemScraper = new(true);
    }
    public Problem? Problem { get; set; }

    public bool DidntFindProblem { get; set; } = false;
    
    public ProblemSearcher ProblemSearcher { get{return _problemSearcher;} }
    
    [BindProperty]
    public string Search { get; set; } = string.Empty;

    public bool ProblemAreadyInDb { get; set; }

    private readonly ILogger<ErrorModel> _logger;

    public AopsImporter(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }


    public IActionResult? OnPost()
    {
        if (!String.IsNullOrEmpty(Search))
        {
            try
            {
                if(Search.Contains("http"))
                {
                    Problem = _problemScraper.ScrapProblemFromAops(Search, new FromSourceIdGenerator());
                }
                else
                {
                    Problem = _problemSearcher.GetFromAops(Search, new FromSourceIdGenerator());
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                DidntFindProblem = true;
            }

            if (!DidntFindProblem)
            {
                try
                {
                    new ProblemManager(new TestDataBaseProvider().GetDatabase()).CreateProblem(Problem);
                    return Redirect("/Problems/" + Problem.Id);
                }
                catch(InvalidOperationException)
                {   
                    _logger.LogError("The problem is already in the database.");
                    ProblemAreadyInDb = true;
                }
            }
        }

        return null;
    }
}