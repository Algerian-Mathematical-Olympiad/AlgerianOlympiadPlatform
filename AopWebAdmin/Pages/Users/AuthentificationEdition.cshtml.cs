using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Users;

public class AuthentificationEditionModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    public AuthentificationEditionModel(IMongoDatabase database)
    {
        _database = database;
    }
    
    [BindProperty(SupportsGet = true)]
    public required string RequestedUser { get; set; }

    [BindProperty] public UserAuthInfo AuthInfo { get; set; } = new();

    public void OnGet()
    {
        AuthInfo = new UserManager(_database).GetUserAuthInfo(RequestedUser);
        AuthInfo.PasswordHash = "";
    }

    public IActionResult OnPost()
    {
        var manager = new UserManager(_database);
        
        AuthInfo.PasswordHash = PasswordHasher.HashPassword(AuthInfo.PasswordHash);
        manager.ChangePassword(AuthInfo);

        return Page();
    }
    
}