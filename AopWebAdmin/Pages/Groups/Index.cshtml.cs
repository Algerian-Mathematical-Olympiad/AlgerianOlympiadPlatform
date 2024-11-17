using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Groups;

public class GroupsModel(IMongoDatabase database) : PageModel
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
    
    public IActionResult OnPost()
    {
        switch (Action)
        {
            case Actions.Delete:
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