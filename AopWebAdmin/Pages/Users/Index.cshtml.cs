using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Users;

[Authorize(Policy = "viewUsers")]
public class UsersModel : PageModel
{
    private readonly IMongoDatabase _database;
    public List<DetailedUser> Users { get; set; } = [];
    [BindProperty]
    public required string RequestedUserEmail { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    private readonly IAuthorizationService _authorizationService;

    public UsersModel(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
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
    
    public async Task<IActionResult> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                var result = await _authorizationService.AuthorizeAsync(User, "DeleteUsers");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
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