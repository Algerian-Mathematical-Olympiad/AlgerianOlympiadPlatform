using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebsite.Pages.Groups;

public class Index : PageModel
{
    public List<Group> Groups { get; set; } = [];
    
    private readonly IMongoDatabase _database;
    public Index(IMongoDatabase database)
    {
        _database = database;
    }
    public void OnGet()
    {
        GetGroups();
    }

    private void GetGroups()
    {
        var username = User.Identity!.Name!;
        Groups = new GroupManager(_database).GetAllGroups()
            .Where(x => x.PublicForStudents || x.Students.Contains(username)).ToList();
    }
}