using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class QuizzesModel : PageModel
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<QuizzesModel> _logger;

    public List<Quiz> Quizzes { get; set; }

    public QuizzesModel(ILogger<QuizzesModel> logger, IMongoDatabase database)
    {
        _logger = logger;
        _database = database;
    }

    public void OnGet()
    {
        GetQuizzes();
    }

    private void GetQuizzes()
    {
        var manager = new QuizManager(_database);
        Quizzes = manager.GetAllQuizzes();
    }

    [BindProperty]
    public string QuizToAffect { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                var manager = new QuizManager(_database);
                manager.DeleteQuiz(QuizToAffect);
                break;
        }

        return Redirect("/Quizzes");
    }

    public enum Actions
    {
        Delete
    }
}