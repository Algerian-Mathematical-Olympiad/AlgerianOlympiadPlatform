using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class AuthentificationEditionModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    public AuthentificationEditionModel(IMongoDatabase database)
    {
        _database = database;
    }
    
    [BindProperty(SupportsGet = true)]
    public string RequestedUser { get; set; }
    [BindProperty] public UserAuthInfo AuthInfo { get; set; }

    public void OnGet()
    {
        AuthInfo = new UserManager(_database).GetUserAuthInfo(RequestedUser);
        AuthInfo.PasswordHash = "";
    }

    public async Task<IActionResult?> OnPostAsync()
    {
        var manager = new UserManager(_database);
        
        AuthInfo.PasswordHash = PasswordHasher.HashPassword(AuthInfo.PasswordHash);
        manager.ChangePassword(AuthInfo);

        return Page();
    }




    
}