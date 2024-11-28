using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Units;

[Authorize(Policy = "ViewUnits")]
public class UnitDetails : PageModel
{
    private readonly IMongoDatabase _database;

    [BindProperty(SupportsGet = true)]
    public required string RequestedUnit { get; set; }

    [BindProperty(SupportsGet = false)] 
    public Unit Unit { get; set; } = new();
    [BindProperty(SupportsGet = false)]
    public Actions Action { get; set; }

    public List<IdOnly> AvailableProblemsets { get; set; } = [];
    public List<IdOnly> AvailableLessons { get; set; } = [];
    public List<IdOnly> AvailableQuizzes { get; set; } = [];

    private readonly IAuthorizationService _authorizationService;

    public UnitDetails(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _database = database;
    }
    public void OnGet()
    {
        if (RequestedUnit == "new")
        {
            FillFieldsOptions();
            return;
        }

        Unit = new UnitManager(_database).GetUnitById(RequestedUnit);
        FillFieldsOptions();
    }

    public async Task<IActionResult?> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Update:
                var result = await _authorizationService.AuthorizeAsync(User, "UpdateUnits");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
                return Update();
            case Actions.Delete:
                var result1 = await _authorizationService.AuthorizeAsync(User, "DeleteUnits");
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
        new UnitManager(_database).DeleteUnit(RequestedUnit);
        return Redirect("/Units");
    }

    private IActionResult? Update()
    {
        if (!ModelState.IsValid || Unit.Id == "new" || Unit.Id == "") return Page();

        var manager = new UnitManager(_database);
        if (RequestedUnit == Unit.Id)
        {
            manager.UpdateUnit(Unit, RequestedUnit);
            Unit = new UnitManager(_database).GetUnitById(RequestedUnit);
            FillFieldsOptions();
            return null;
        }
        else
        {
            if (manager.UnitExists(Unit.Id))
            {
                throw new Exception($"Unit with id {Unit.Id} already exists");
            }
            manager.CreateUnit(Unit);
            return Redirect("/Units/" + Unit.Id);
        }

    }

    private void FillFieldsOptions()
    {
        FillAvailableProblemsets();
        FillAvailableLessons();
        FillAvailableQuizzes();
    }

    private void FillAvailableQuizzes()
    {
        var manager = new QuizManager(_database);
        AvailableQuizzes = manager.GetQuizzesIds().Where(x => !Unit.ValidationQuizzes.Contains(x.Id)).ToList();
    }

    private void FillAvailableLessons()
    {
        var manager = new LessonManager(_database);
        AvailableLessons = manager.GetLessonsIds().Where(x => !Unit.Lessons.Contains(x.Id)).ToList();
    }

    private void FillAvailableProblemsets()
    {
        var problemsetManager = new ProblemsetManager(_database);
        AvailableProblemsets = problemsetManager.GetProblemsetsIds();
    }

    public enum Actions
    {
        Delete,
        Update
    }
}
