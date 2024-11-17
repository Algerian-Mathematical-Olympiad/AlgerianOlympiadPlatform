using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Problems;

public class ProblemsModel : PageModel
{
    private readonly IMongoDatabase _database;

    public List<Problem> Problems { get; set; } = [];
    
    [BindProperty]
    public required string RequestedProblem { get; set; }
    
    [BindProperty]
    public Actions Action { get; set; }


    public ProblemsModel(IMongoDatabase database)
    {
        _database = database;
    }

    public void OnGet()
    {
        GetProblems();
    }

    private void GetProblems()
    {
        var u = new ProblemManager(_database);
        Problems = u.GetAllProblems();
        
    }
    
    public IActionResult OnPost()
    {
        switch (Action)
        {
            case Actions.Delete:
                var u = new ProblemManager(_database);
                u.DeleteProblem(RequestedProblem);
                break;
        }
        
        return Redirect("/Problems"); 
    }


    public enum Actions
    {
        Delete
    }
}