using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Exams;

public class ExamsModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    [BindProperty]
    public required string RequestedExam { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    public List<Exam> Exams { get; set; } = [];

    public ExamsModel(IMongoDatabase database)
    {
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

    public IActionResult OnPost()
    {
        switch (Action)
        {
            case Actions.Delete:
                var manager = new ExamManager(_database);
                manager.DeleteExam(RequestedExam);
                break;
        }

        return Redirect("/Exams");
    }

    public enum Actions
    {
        Delete
    }
}