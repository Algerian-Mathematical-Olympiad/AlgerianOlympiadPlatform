using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class SubmissionsModel : PageModel
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<SubmissionsModel> _logger;

    public List<StudentSubmission> Submissions { get; set; }

    public SubmissionsModel(ILogger<SubmissionsModel> logger, IMongoDatabase database)
    {
        _logger = logger;
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

    [BindProperty]
    public string SubmissionToAffect { get; set; }
    
    [BindProperty]
    public Actions Action { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                new StudentSubmissionManager(_database).DeleteStudentSubmission(SubmissionToAffect);
                break;
        }
        return RedirectToPage("/Problems/Submissions/Index");
    }

    public enum Actions
    {
        Delete
    }
}