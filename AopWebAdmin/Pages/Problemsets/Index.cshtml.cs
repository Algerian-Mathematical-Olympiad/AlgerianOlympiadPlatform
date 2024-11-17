using DatabaseConnector;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Problemsets;

public class ProblemsetsModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    public ProblemsetsModel(IMongoDatabase database)
    {
        _database = database;
    }

    public List<MathProblemset> Problemsets { get; set; }
    public string ProblemsetToAffect { get; set; }
    
    public Actions Action { get; set; }

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