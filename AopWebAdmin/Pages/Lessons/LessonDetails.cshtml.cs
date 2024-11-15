using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class LessonDetails : PageModel
{
    private readonly IMongoDatabase _database;
    
    public LessonDetails(IMongoDatabase database)
    {
        _database = database;
    }
    
    [BindProperty(SupportsGet = true)]
    public string RequestedLesson { get; set; }
    [BindProperty(SupportsGet = false)]
    public Lesson Lesson { get; set; }
    
    [BindProperty(SupportsGet = false)]
    public Actions Action { get; set; }

    public List<Asset> AvailableAttachements { get; set; } = [];

    
    public void OnGet()
    {
        FillAvailableAttachements();
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
        new LessonManager(_database).DeleteLesson(RequestedLesson);
        return Redirect("/Lessons");
    }

    private IActionResult? Update()
    {
        if (!ModelState.IsValid || Lesson.Id == "new") return Page();
        
        if (RequestedLesson == Lesson.Id)
        {
            new LessonManager(_database).UpdateLesson(Lesson, RequestedLesson);
            FillAvailableAttachements();
            return null;
        }
        else
        {
            var manager = new LessonManager(_database);
            if (manager.LessonExists(Lesson.Id))
            {
                throw new Exception($"Lesson with id {Lesson.Id} already exists");
            }
            if(RequestedLesson != "new") manager.DeleteLesson(RequestedLesson);
            manager.CreateLesson(Lesson);
            return Redirect("/Lessons/"+Lesson.Id);
        }
    }
    
    private void FillAvailableAttachements()
    {
        if (RequestedLesson == "new")
        {
            Lesson = new();
            AvailableAttachements = new CloudAssetManager(_database).GetAllAssets();
            return;
        }
        
        Lesson = new LessonManager(_database).GetLessonById(RequestedLesson);

        AvailableAttachements = new CloudAssetManager(_database).GetAllAssets().Where(only => !Lesson.Attachments.Contains(only.Id)).ToList();
    }

    public enum Actions
    {
        Delete,
        Update
    }
}