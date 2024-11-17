using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Users;

public class UsersModel : PageModel
{
    private readonly IMongoDatabase _database;
    public List<DetailedUser> Users { get; set; } = [];
    [BindProperty]
    public required string RequestedUserEmail { get; set; }
    [BindProperty]
    public Actions Action { get; set; }


    public UsersModel(IMongoDatabase database)
    {
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
        Users = users;
    }
    
    public IActionResult OnPost()
    {
        switch (Action)
        {
            case Actions.Delete:
                var u = new UserManager(_database);
                u.DeleteUser(RequestedUserEmail);
                break;
        }
        
        return RedirectToPage("/Users/Index"); 
    }


    public enum Actions
    {
        Delete
    }
}