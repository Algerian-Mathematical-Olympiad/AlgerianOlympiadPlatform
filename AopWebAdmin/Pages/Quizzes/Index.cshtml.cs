using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Quizzes;

[Authorize(Policy = "ViewQuizzes")]
public class QuizzesModel : PageModel
{
    private readonly IMongoDatabase _database;

    public List<Quiz> Quizzes { get; set; } = [];
    
    [BindProperty]
    public required string RequestedQuiz { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    private readonly IAuthorizationService _authorizationService;
    public QuizzesModel(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
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
    
    public async Task<IActionResult> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                var result = await _authorizationService.AuthorizeAsync(User, "DeleteQuizzes");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
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