using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Groups;

[Authorize(Policy = "ViewGroups")]
public class GroupsModel(IMongoDatabase database, IAuthorizationService authorizationService) : PageModel
{
    [BindProperty]
    public required string RequestedGroup { get; set; }
    [BindProperty]
    public Actions Action { get; set; }
    
    public List<Group> Groups { get; set; } = [];

    public void OnGet()
    {
        GetGroups();
    }

    private void GetGroups()
    {
        var u = new GroupManager(database);
        Groups = u.GetAllGroups();
        
    }
    
    public async Task<IActionResult> OnPost()
    {
        switch (Action)
        {
            case Actions.Delete:
                var result = await authorizationService.AuthorizeAsync(User, "DeleteGroups");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
                var u = new GroupManager(database);
                u.DeleteGroup(RequestedGroup);
                break;
        }
        
        return Redirect("/Groups"); 
    }


    public enum Actions
    {
        Delete
    }
}