using AopsUtilities;
using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenQA.Selenium.Interactions;

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
    
    [BindProperty]
    public bool Batch {get; set;}
    [BindProperty]
    public string ContestName {get; set;} = string.Empty;
    [BindProperty]
    public int FirstYear {get; set;}
    [BindProperty]
    public int LastYear {get; set;}
    [BindProperty]
    public int NumberOfProblemsPerContest {get; set;}

    public AopsImporter(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }


    public IActionResult? OnPost()
    {
        if (!Batch)
        {
            return ImportOneProblem();
        }

        return ImportWholeContest();
    }

    private RedirectToPageResult? ImportWholeContest()
    {
        var manager = new ProblemManager(new TestDataBaseProvider().GetDatabase());
        for (int year = FirstYear; year <= LastYear; year++)
        {
            for (int p = 1; p <= NumberOfProblemsPerContest; p++)
            {
                try
                {
                    var pb = _problemSearcher.GetFromAops(ContestName + " " + year + " P" + p, new FromSourceIdGenerator());
                    manager.CreateProblem(pb);
                }catch{}
            }
        }
        
        return RedirectToPage("/Problems/Index");
    }

    private IActionResult? ImportOneProblem()
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