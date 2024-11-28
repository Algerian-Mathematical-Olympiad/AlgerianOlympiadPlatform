using DatabaseConnector;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Problemsets;

[Authorize(Policy = "ViewProblemsets")]
public class ProblemsetsModel : PageModel
{
    private readonly IMongoDatabase _database;

    public List<MathProblemset> Problemsets { get; set; } = [];
    public required string RequestedProblemset { get; set; }
    
    public Actions Action { get; set; }
    
    private readonly IAuthorizationService _authorizationService;
    public ProblemsetsModel(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _database = database;
    }

    public void OnGet()
    {
        GetProblemsets();
    }

    private void GetProblemsets()
    {
        var u = new ProblemsetManager(_database);
        Problemsets = u.GetMathProblemsets();
    }

    public enum Actions
    {
        Delete
    }

}