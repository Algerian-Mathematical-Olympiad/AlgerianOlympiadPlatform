using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class GroupsModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    private readonly ILogger<GroupsModel> _logger;

    public List<Group> AopGroups { get; set; }

    public GroupsModel(ILogger<GroupsModel> logger, IMongoDatabase database)
    {
        _logger = logger;
        _database = database;
    }

    public void OnGet()
    {
        GetGroups();
    }

    private void GetGroups()
    {
        var u = new GroupManager(_database);
        AopGroups = u.GetAllGroups();
        
    }
    
    [BindProperty]
    public string GroupToAffect { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                var u = new GroupManager(_database);
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