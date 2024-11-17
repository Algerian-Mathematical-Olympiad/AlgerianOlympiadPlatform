using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Problems;

public class ProblemDetails : PageModel
{
    private readonly IMongoDatabase _database;

    [BindProperty(SupportsGet = true)] 
    public required string RequestedProblem { get; set; }

    [BindProperty(SupportsGet = false)] 
    public Problem Problem { get; set; } = new();

    [BindProperty(SupportsGet = false)] 
    public Actions Action { get; set; }

    public ProblemDetails(IMongoDatabase database)
    {
        _database = database;
    }

    public void OnGet()
    {
        if (RequestedProblem != "new")
        {
            Problem = new ProblemManager(_database).GetProblemById(RequestedProblem);
        }
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
        new ProblemManager(_database).DeleteProblem(RequestedProblem);
        return Redirect("/Problems");
    }

    private IActionResult? Update()
    {
        if (!ModelState.IsValid || Problem.Id == "new") return Page();

        if (RequestedProblem == Problem.Id)
        {
            new ProblemManager(_database).UpdateProblem(Problem, RequestedProblem);
            return null;
        }
        else
        {
            var manager = new ProblemManager(_database);
            if (manager.ProblemExists(Problem.Id))
            {
                throw new Exception($"Problem with id {Problem.Id} already exists");
            }

            if (RequestedProblem != "new") manager.DeleteProblem(RequestedProblem);
            manager.CreateProblem(Problem);
            return Redirect("/Problems/" + Problem.Id);
        }
    }

    public enum Actions
    {
        Delete,
        Update
    }
}