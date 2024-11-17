using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class SendEmailModel : PageModel
{
    [BindProperty]
    public required string ToEmail { get; set; }
    [BindProperty]
    public required string Subject { get; set; }
    [BindProperty]
    public required string Message { get; set; }

    public List<User> Users { get; set; } = [];

    public string? ErrorMessage { get; set; }


    private readonly EmailService _emailService;
    
    private IMongoDatabase _database;

    public SendEmailModel(EmailService emailService, IMongoDatabase database)
    {
        _emailService = emailService;
        _database = database;
    }

    public void OnGet()
    {
        GetUsers();
    }
    
    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            await _emailService.SendEmailAsync(ToEmail, Subject, Message);
            ErrorMessage = "Email sent successfully!";
        }
        GetUsers();
        return Page();
    }

    private void GetUsers()
    {
        var manager = new UserManager(_database);  
        Users = manager.GetUsers();
    }

}