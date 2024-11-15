using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class UsersModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    private readonly ILogger<UsersModel> _logger;

    public List<DetailedUser> AopUsers { get; set; }

    public UsersModel(ILogger<UsersModel> logger, IMongoDatabase database)
    {
        _logger = logger;
        _database = database;
    }

    public void OnGet()
    {
        GetUsers();
    }

    private void GetUsers()
    {
        var u = new UserManager(_database);

        var users = u.GetUsersWithDetails();
        AopUsers = users;
    }
    
    [BindProperty]
    public string UserEmailToAffect { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                var u = new UserManager(_database);
                u.DeleteUser(UserEmailToAffect);
                break;
        }
        
        return RedirectToPage("/Users/Index"); 
    }


    public enum Actions
    {
        Delete
    }
}