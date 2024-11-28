using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Exams;

public class ExamDetails : PageModel
{
    private readonly IMongoDatabase _database;
    private readonly IAuthorizationService _authorizationService;

    public ExamDetails(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _database = database;
    }

    [BindProperty(SupportsGet = true)]
    public required string RequestedExam { get; set; }

    [BindProperty(SupportsGet = false)] 
    public Exam Exam { get; set; } = new();

    [BindProperty(SupportsGet = false)]
    public Actions Action { get; set; }

    public List<IdOnly> AvailableProblemsets { get; set; } = [];

    public List<UserPermissions> AvailableCorrectors { get; set; } = [];
    public List<IdOnly> AvailableGroups { get; set; } = [];

    public void OnGet()
    {
        GetExam();
        FillListsData();
    }

    public async Task<IActionResult?> OnPost()
    {
        switch (Action)
        {
            case Actions.Update:
                var result = await _authorizationService.AuthorizeAsync(User, "UpdateExams");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
                return Update();
            case Actions.Delete:
                var result1 = await _authorizationService.AuthorizeAsync(User, "UpdateExams");
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
        new ExamManager(_database).DeleteExam(RequestedExam);
        return Redirect("/Exams");
    }

    private IActionResult? Update()
    {
        if (!ModelState.IsValid || Exam.Id == "new") return Page();

        var manager = new ExamManager(_database);

        if (RequestedExam == Exam.Id)
        {
            manager.UpdateExam(Exam, RequestedExam);
            FillListsData();
            return null;
        }
        else
        {
            if (manager.ExamExists(Exam.Id))
            {
                throw new Exception($"Exam with ID {Exam.Id} already exists.");
            }
            if (RequestedExam != "new") manager.DeleteExam(RequestedExam);
            manager.CreateExam(Exam);
            return Redirect("/Exams/" + Exam.Id);
        }
    }

    private void GetExam()
    {
        if (RequestedExam != "new")
        {
            Exam = new ExamManager(_database).GetExamById(RequestedExam);
        }
    }

    private void FillListsData()
    {
        FillAvailableCorrectors();
        FillAvailableProblemsets();
        FillAvailableGroups();
    }

    private void FillAvailableGroups()
    {
        AvailableGroups = new GroupManager(_database).GetGroupsIds().Where(x=>!Exam.GroupsConcerned.Contains(x.Id)).ToList();
    }
    private void FillAvailableCorrectors()
    {
        var userManager = new UserPermissionsManager(_database);
        AvailableCorrectors = userManager.GetStaff().Where(x => !Exam.Correctors.Contains(x.Id)).ToList();
    }

    private void FillAvailableProblemsets()
    {
        AvailableProblemsets = new ProblemsetManager(_database).GetProblemsetsIds();
    }

    public enum Actions
    {
        Delete,
        Update
    }
}
