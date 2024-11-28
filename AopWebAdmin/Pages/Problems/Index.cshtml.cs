using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Problems;

[Authorize(Policy = "ViewProblems")]
public class ProblemsModel : PageModel
{
    private readonly IMongoDatabase _database;

    public List<Problem> Problems { get; set; } = [];
    
    [BindProperty]
    public required string RequestedProblem { get; set; }
    
    [BindProperty]
    public Actions Action { get; set; }

    private readonly IAuthorizationService _authorizationService;

    public ProblemsModel(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
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
    
    public async Task<IActionResult> OnPost()
    {
        switch (Action)
        {
            case Actions.Delete:
                var result = await _authorizationService.AuthorizeAsync(User, "DeleteProblems");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
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