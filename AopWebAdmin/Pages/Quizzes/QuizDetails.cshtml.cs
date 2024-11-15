using System.Text.Json;
using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Quizzes;

public class QuizDetails : PageModel
{
    private readonly IMongoDatabase _database;

    public QuizDetails(IMongoDatabase database)
    {
        _database = database;
    }
    
    [BindProperty(SupportsGet = true)]
    public string RequestedQuiz { get; set; }
    [BindProperty(SupportsGet = false)]
    public Quiz Quiz { get; set; }
    
    [BindProperty(SupportsGet = false)]
    public Actions Action { get; set; }


    public void OnGet()
    {
        if (RequestedQuiz == "new")
        {
            Quiz = new()
            {
                Id = "",
                //Answers = [],
                Name = new(),
                Points = 10,
                Statement = new(),
                //CorrectAnswers = []
            };
            return;
        }
        
        Quiz = new QuizManager(_database).GetQuizById(RequestedQuiz);

    }

    public IActionResult? OnPost()
    {
        switch (Action)
        {
            case Actions.Update:
                return Update();
            case Actions.Delete:
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
