using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Quizzes;

[Authorize(Policy = "ViewQuizzes")]
public class QuizDetails : PageModel
{
    private readonly IMongoDatabase _database;
    
    [BindProperty(SupportsGet = true)]
    public required string RequestedQuiz { get; set; }

    [BindProperty(SupportsGet = false)] 
    public Quiz Quiz { get; set; } = new();
    
    [BindProperty(SupportsGet = false)]
    public Actions Action { get; set; }

    private readonly IAuthorizationService _authorizationService;

    public QuizDetails(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _database = database;
    }

    public void OnGet()
    {
        GetQuiz();
    }

    private void GetQuiz()
    {
        if (RequestedQuiz != "new")
        {
            Quiz = new QuizManager(_database).GetQuizById(RequestedQuiz);
        }
    }

    public async Task<IActionResult?> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Update:
                var result = await _authorizationService.AuthorizeAsync(User, "UpdateQuizzes");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
                return Update();
            case Actions.Delete:
                var result1 = await _authorizationService.AuthorizeAsync(User, "DeleteQuizzes");
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
        new QuizManager(_database).DeleteQuiz(RequestedQuiz);
        return Redirect("/Quizzes");
    }

    private IActionResult? Update()
    {
        CleanAnswers();

        if (Quiz.Id == "new") return Page();
        
        if (RequestedQuiz == Quiz.Id)
        {
            new QuizManager(_database).UpdateQuiz(Quiz, RequestedQuiz);
            return null;
        }
        else
        {
            var manager = new QuizManager(_database);
            if (manager.QuizExists(Quiz.Id))
            {
                throw new Exception($"Quiz with id {Quiz.Id} already exists");
            }
            if(RequestedQuiz != "new") manager.DeleteQuiz(RequestedQuiz);
            manager.CreateQuiz(Quiz);
            return Redirect("/Quizzes/"+Quiz.Id);
        }
    }

    private void CleanAnswers()
    {
        Quiz.Answers = Quiz.Answers
            .Where(x =>
            {
                try
                {
                    if (string.IsNullOrEmpty(x.Value.Descriptions.EnglishDescription.Content)) return false;
                }
                catch
                {
                    return false;
                }
                Console.WriteLine(x.Value.Descriptions.EnglishDescription.Content);
                return true;
            })
            .ToDictionary();
    }



    public enum Actions
    {
        Delete,
        Update
    }
}
