using DatabaseConnector;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Problemsets;

public class ProblemsetsModel : PageModel
{
    private readonly IMongoDatabase _database;

    public List<MathProblemset> Problemsets { get; set; } = [];
    public required string RequestedProblemset { get; set; }
    
    public Actions Action { get; set; }
    
    public ProblemsetsModel(IMongoDatabase database)
    {
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