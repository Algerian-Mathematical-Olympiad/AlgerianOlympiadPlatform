using AopsUtilities;
using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

[Authorize(Policy = "EditProblems")]
public class AopsImporter : PageModel
{
    private static ProblemSearcher? _problemSearcher;
    private static ProblemImporter? _problemScraper;
    private readonly IMongoDatabase _database;



    public static void Init(bool initScraper = false)
    {
        _problemSearcher = new();
        if(initScraper)
            _problemScraper = new(true);
    }
    public Problem? Problem { get; set; }

    public bool DidntFindProblem { get; set; } 
    
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

    public AopsImporter(ILogger<ErrorModel> logger,IMongoDatabase database)
    {
        _database = database;
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

    private RedirectToPageResult ImportWholeContest()
    {
        var manager = new ProblemManager(_database);
        for (int year = FirstYear; year <= LastYear; year++)
        {
            for (int p = 1; p <= NumberOfProblemsPerContest; p++)
            {
                try
                {
                    var pb = _problemSearcher?.GetFromAops(ContestName + " " + year + " P" + p, new FromSourceIdGenerator());
                    if (pb != null) manager.CreateProblem(pb);
                }
                catch
                {
                    // ignore if problem not found
                }
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
                Problem = Search.Contains("http") && _problemScraper != null
                    ? _problemScraper.ScrapProblemFromAops(Search, new FromSourceIdGenerator()) 
                    : _problemSearcher?.GetFromAops(Search, new FromSourceIdGenerator());
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
                    if (Problem != null)
                    {
                        new ProblemManager(_database).CreateProblem(Problem);
                        return Redirect("/Problems/" + Problem.Id);
                    }
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