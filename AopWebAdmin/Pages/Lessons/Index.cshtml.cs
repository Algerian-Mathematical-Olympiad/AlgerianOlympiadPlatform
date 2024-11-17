using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Lessons;

public class LessonsModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    
    [BindProperty]
    public required string RequestedLesson { get; set; }
    [BindProperty]
    public Actions Action { get; set; }


    public List<Lesson> Lessons { get; set; } = [];

    public LessonsModel(IMongoDatabase database)
    {
        _database = database;
    }

    public void OnGet()
    {
        GetLessons();
    }

    private void GetLessons()
    {
        var u = new LessonManager(_database);
        Lessons = u.GetAllLessons();
        
    }
    
    public IActionResult OnPost()
    {
        switch (Action)
        {
            case Actions.Delete:
                var u = new LessonManager(_database);
                u.DeleteLesson(RequestedLesson);
                break;
        }
        
        return Redirect("/Lessons"); 
    }


    public enum Actions
    {
        Delete
    }
}