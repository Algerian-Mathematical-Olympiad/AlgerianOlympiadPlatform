using DatabaseConnector;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AopWebAdmin.Pages.Problemsets;

public class ProblemsetsModel : PageModel
{
    public List<MathProblemset> AopProblemsets { get; set; }
    public string ProblemsetToAffect { get; set; }
    
    public Actions Action { get; set; }

    public void OnGet()
    {
        GetProblemsets();
    }

    private void GetProblemsets()
    {
        var u = new ProblemsetManager(new TestDataBaseProvider().GetDatabase());
        AopProblemsets = u.GetMathProblemsets();
    }

    public enum Actions
    {
        Delete
    }

}