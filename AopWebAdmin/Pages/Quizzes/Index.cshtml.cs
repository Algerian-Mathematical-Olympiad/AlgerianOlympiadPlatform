using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Quizzes;

public class QuizzesModel : PageModel
{
    private readonly IMongoDatabase _database;

    public List<Quiz> Quizzes { get; set; } = [];
    
    [BindProperty]
    public required string RequestedQuiz { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    public QuizzesModel(IMongoDatabase database)
    {
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
    
    public IActionResult OnPost()
    {
        switch (Action)
        {
            case Actions.Delete:
                var manager = new QuizManager(_database);
                manager.DeleteQuiz(RequestedQuiz);
                break;
        }

        return Redirect("/Quizzes");
    }

    public enum Actions
    {
        Delete
    }
}