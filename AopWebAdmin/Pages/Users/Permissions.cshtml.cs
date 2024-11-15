using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class PermissionsModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    public PermissionsModel(IMongoDatabase database)
    {
        _database = database;
    }
    
    [BindProperty(SupportsGet = true)]
    public string RequestedUser { get; set; }
    [BindProperty] public UserPermissions Permissions { get; set; }

    public void OnGet()
    {
        Permissions = new UserPermissionsManager(_database).GetUserPermissionsById(RequestedUser);
    }

    public async Task<IActionResult?> OnPostAsync()
    {
        //if (!ModelState.IsValid) return Page();

        var manager = new UserPermissionsManager(_database);
        
        manager.UpdateUserPermissions(Permissions, RequestedUser);

        return Page();
    }




    
}