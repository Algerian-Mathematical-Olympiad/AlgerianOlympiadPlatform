using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Problems.Submissions;

public class SubmissionsModel : PageModel
{
    private readonly IMongoDatabase _database;

    public List<StudentSubmission> Submissions { get; set; } = [];
    
    [BindProperty]
    public required string RequestedSubmission { get; set; }
    
    [BindProperty]
    public Actions Action { get; set; }


    public SubmissionsModel(IMongoDatabase database)
    {
        _database = database;
    }

    public void OnGet()
    {
        LoadSubmissions();
    }

    private void LoadSubmissions()
    {
        Submissions = new StudentSubmissionManager(_database).GetStudentSubmissions();
    }
    
    public IActionResult OnPost()
    {
        switch (Action)
        {
            case Actions.Delete:
                new StudentSubmissionManager(_database).DeleteStudentSubmission(RequestedSubmission);
                break;
        }
        return RedirectToPage("/Problems/Submissions/Index");
    }

    public enum Actions
    {
        Delete
    }
}