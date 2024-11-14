using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class ProblemsModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    private readonly ILogger<ProblemsModel> _logger;

    public List<Problem> AopProblems { get; set; }

    public ProblemsModel(ILogger<ProblemsModel> logger, IMongoDatabase database)
    {
        _logger = logger;
        _database = database;
    }

    public void OnGet()
    {
        GetProblems();
    }

    private void GetProblems()
    {
        var u = new ProblemManager(_database);
        AopProblems = u.GetAllProblems();
        
    }
    
    [BindProperty]
    public string ProblemToAffect { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                var u = new ProblemManager(_database);
                u.DeleteProblem(ProblemToAffect);
                break;
        }
        
        return Redirect("/Problems"); 
    }


    public enum Actions
    {
        Delete
    }
}