using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class ExamsModel : PageModel
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<ExamsModel> _logger;

    public List<Exam> Exams { get; set; }

    public ExamsModel(ILogger<ExamsModel> logger, IMongoDatabase database)
    {
        _logger = logger;
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

    [BindProperty]
    public string ExamToAffect { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                var manager = new ExamManager(_database);
                manager.DeleteExam(ExamToAffect);
                break;
        }

        return Redirect("/Exams");
    }

    public enum Actions
    {
        Delete
    }
}