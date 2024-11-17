using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class LessonsModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    private readonly ILogger<LessonsModel> _logger;

    public List<Lesson> Lessons { get; set; }

    public LessonsModel(ILogger<LessonsModel> logger, IMongoDatabase database)
    {
        _logger = logger;
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
    
    [BindProperty]
    public string LessonToAffect { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                var u = new LessonManager(_database);
                u.DeleteLesson(LessonToAffect);
                break;
        }
        
        return Redirect("/Lessons"); 
    }


    public enum Actions
    {
        Delete
    }
}