using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Groups;

public class GroupsModel(IMongoDatabase database) : PageModel
{
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
    
    [BindProperty]
    public required string GroupToAffect { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    public IActionResult OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                var u = new GroupManager(database);
                u.DeleteGroup(GroupToAffect);
                break;
        }
        
        return Redirect("/Groups"); 
    }


    public enum Actions
    {
        Delete
    }
}