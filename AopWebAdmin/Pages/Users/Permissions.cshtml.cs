using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Users;

public class PermissionsModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    [BindProperty(SupportsGet = true)]
    public required string RequestedUser { get; set; }

    [BindProperty] public UserPermissions Permissions { get; set; } = new();

    public List<Permission> SelectedPermissions { get; set; } = new();
    public PermissionsModel(IMongoDatabase database)
    {
        _database = database;
    }


    public void OnGet()
    {
        Permissions = new UserPermissionsManager(_database).GetUserPermissionsById(RequestedUser);
    }

    public void UpdatePermission(Permission permission)
    {
        if(SelectedPermissions.Contains(permission))
        {
            SelectedPermissions.Remove(permission);
        }
        else
        {
            SelectedPermissions.Add(permission);
        }
    }

    public IActionResult OnPost()
    {
        var manager = new UserPermissionsManager(_database);
        
        Permissions.Permissions.AddRange(SelectedPermissions);        
        
        manager.UpdateUserPermissions(Permissions, RequestedUser);

        return Page();
    }
    
}