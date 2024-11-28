using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Problemsets;

[Authorize(Policy = "ViewProblemsets")]
public class ProblemsetDetails : PageModel
{
    private readonly IMongoDatabase _database;
    
    [BindProperty(SupportsGet = true)]
    public required string RequestedProblemset { get; set; }

    [BindProperty(SupportsGet = false)] 
    public MathProblemset Problemset { get; set; } = new();
    
    [BindProperty(SupportsGet = false)]
    public Actions Action { get; set; }

    public List<IdOnly> AvailableProblems { get; set; } = [];

    private readonly IAuthorizationService _authorizationService;
    
    public ProblemsetDetails(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _database = database;
    }

    public void OnGet()
    {
        GetProblemset();
        FillAvailableProblems();
    }

    public async Task<IActionResult?> OnPost()
    {
        switch (Action)
        {
            case Actions.Update:
                var result = await _authorizationService.AuthorizeAsync(User, "UpdateProblemsets");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
                return Update();
            case Actions.Delete:
                var result1 = await _authorizationService.AuthorizeAsync(User, "DeleteProblemsets");
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
        new ProblemsetManager(_database).DeleteProblemset(RequestedProblemset);
        return Redirect("/Problemsets");
    }

    private IActionResult? Update()
    {
        if (!ModelState.IsValid || Problemset.Id == "new") return Page();
        
        if (RequestedProblemset == Problemset.Id)
        {
            new ProblemsetManager(_database).UpdateProblemset(Problemset, RequestedProblemset);
            FillAvailableProblems();
            return null;
        }
        else
        {
            var manager = new ProblemsetManager(_database);
            if (manager.ProblemsetExists(Problemset.Id))
            {
                throw new Exception($"Problemset with id {Problemset.Id} already exists");
            }
            if(RequestedProblemset != "new") manager.DeleteProblemset(RequestedProblemset);
            manager.CreateProblemset(Problemset);
            return Redirect("/Problemsets/"+Problemset.Id);
        }
    }

    private void GetProblemset()
    {
        if (RequestedProblemset != "new")
        {
            Problemset = new ProblemsetManager(_database).GetProblemsetById(RequestedProblemset);
        }
    }

    private void FillAvailableProblems()
    {
        if (RequestedProblemset == "new")
        { 
            AvailableProblems = new ProblemManager(_database).GetProblemsIds();
            return;
        }
        
        AvailableProblems = new ProblemManager(_database).GetProblemsIds().Where(only => !Problemset.ProblemsIds.Contains(only.Id)).ToList();
    }

    public enum Actions
    {
        Delete,
        Update
    }
}