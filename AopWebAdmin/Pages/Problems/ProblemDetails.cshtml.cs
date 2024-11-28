using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Problems;

[Authorize(Policy = "ViewProblems")]
public class ProblemDetails : PageModel
{
    private readonly IMongoDatabase _database;

    [BindProperty(SupportsGet = true)] 
    public required string RequestedProblem { get; set; }

    [BindProperty(SupportsGet = false)] 
    public Problem Problem { get; set; } = new();

    [BindProperty(SupportsGet = false)] 
    public Actions Action { get; set; }

    private readonly IAuthorizationService _authorizationService;

    public ProblemDetails(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _database = database;
    }

    public void OnGet()
    {
        if (RequestedProblem != "new")
        {
            Problem = new ProblemManager(_database).GetProblemById(RequestedProblem);
        }
    }

    public async Task<IActionResult?> OnPost()
    {
        switch (Action)
        {
            case Actions.Update:
                var result = await _authorizationService.AuthorizeAsync(User, "UpdateProblems");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
                return Update();
            case Actions.Delete:
                var result1 = await _authorizationService.AuthorizeAsync(User, "DeleteProblems");
                if(!result1.Succeeded)
                {
                    return Redirect("/");
                }
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