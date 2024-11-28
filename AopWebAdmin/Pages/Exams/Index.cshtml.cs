using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Exams;

[Authorize("ViewExams")]
public class ExamsModel : PageModel
{
    private readonly IMongoDatabase _database;
    private readonly IAuthorizationService _authorizationService;
    
    [BindProperty]
    public required string RequestedExam { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    public List<Exam> Exams { get; set; } = [];

    public ExamsModel(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _database = database;
    }

    public void OnGet()
    {
        GetExams();
    }

    private void GetExams()
    {
        var manager = new ExamManager(_database);
        Exams = manager.GetAllExams();
    }

    public async Task<IActionResult> OnPost()
    {
        switch (Action)
        {
            case Actions.Delete:
                var result = await _authorizationService.AuthorizeAsync(User, "SendEmails");
                if(result.Succeeded)
                {
                    var manager = new ExamManager(_database);
                    manager.DeleteExam(RequestedExam);
                }
                else
                {
                    return Redirect("/Exams");
                }
                break;
        }

        return Redirect("/Exams");
    }

    public enum Actions
    {
        Delete
    }
}