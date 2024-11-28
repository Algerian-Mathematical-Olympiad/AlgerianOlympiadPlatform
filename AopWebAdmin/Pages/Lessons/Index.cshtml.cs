using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Lessons;

[Authorize(Policy = "ViewLessons")]
public class LessonsModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    
    [BindProperty]
    public required string RequestedLesson { get; set; }
    [BindProperty]
    public Actions Action { get; set; }
    private readonly IAuthorizationService _authorizationService;

    public List<Lesson> Lessons { get; set; } = [];

    public LessonsModel(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
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
    
    public async Task<IActionResult> OnPost()
    {
        switch (Action)
        {
            case Actions.Delete:
                var result = await _authorizationService.AuthorizeAsync(User, "DeleteLessons");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
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