using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebsite.Pages.Groups.Units;

public class Index : PageModel
{
    private readonly IMongoDatabase _database;
    public List<Unit> Units { get; set; } = [];
    [BindProperty(SupportsGet = true)]
    public required string RequestedGroup { get; set; }
    public Group Group { get; set; }


        
    public Index(IMongoDatabase database)
    {
        _database = database;
    }
    
    public void OnGet()
    {
        GetUnits();
    }

    private void GetUnits()
    {
        Group = new GroupManager(_database).GetGroupById(RequestedGroup);
        Units = new UnitManager(_database).GetUnits().Where(x => Group.Units.Contains(x.Id)).ToList();
    }

}