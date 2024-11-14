using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AopWebAdmin.Pages;

public class ProblemDetails : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string RequestedProblem { get; set; }
    [BindProperty(SupportsGet = false)]
    public Problem Problem { get; set; }
    
    [BindProperty(SupportsGet = false)]
    public Actions Action { get; set; }
    
    public void OnGet()
    {
        if (RequestedProblem == "new")
        {
            Problem = new(
            
                id : "",
                difficulty : new(1,1),
                descriptions : new()
                {
                    ArabicDescription = new(""),
                    EnglishDescription = new ("")
                },
                source : new("","")
            );
            return;
        }
        
        Problem = new ProblemManager(new TestDataBaseProvider().GetDatabase()).GetProblemById(RequestedProblem);
    }

    public IActionResult? OnPost()
    {
        switch (Action)
        {
            case Actions.Update:
                return Update();
            case Actions.Delete:
                return Delete();
        }

        return null;
    }

    private IActionResult Delete()
    {
        new ProblemManager(new TestDataBaseProvider().GetDatabase()).DeleteProblem(RequestedProblem);
        return Redirect("/Problems");
    }

    private IActionResult? Update()
    {
        if (!ModelState.IsValid || Problem.Id == "new") return Page();
        
        if (RequestedProblem == Problem.Id)
        {
            new ProblemManager(new TestDataBaseProvider().GetDatabase()).UpdateProblem(Problem, RequestedProblem);
            return null;
        }
        else
        {
            var manager = new ProblemManager(new TestDataBaseProvider().GetDatabase());
            if (manager.ProblemExists(Problem.Id))
            {
                throw new Exception($"Problem with id {Problem.Id} already exists");
            }
            if(RequestedProblem != "new") manager.DeleteProblem(RequestedProblem);
            manager.CreateProblem(Problem);
            return Redirect("/Problems/"+Problem.Id);
        }
    }

    public enum Actions
    {
        Delete,
        Update
    }
}