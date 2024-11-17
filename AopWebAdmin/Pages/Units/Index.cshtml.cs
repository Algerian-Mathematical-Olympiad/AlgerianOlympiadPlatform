using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Units;

public class UnitsModel : PageModel
{
    private readonly IMongoDatabase _database;

    public List<Unit> Units { get; set; } = [];
    
    [BindProperty]
    public required string RequestedUnit { get; set; }
    [BindProperty]
    public Actions Action { get; set; }


    public UnitsModel(IMongoDatabase database)
    {
        _database = database;
    }

    public void OnGet()
    {
        GetUnits();
    }

    private void GetUnits()
    {
        var unitManager = new UnitManager(_database);
        Units = unitManager.GetUnits();
    }
    
    public IActionResult OnPost()
    {
        switch (Action)
        {
            case Actions.Delete:
                var unitManager = new UnitManager(_database);
                unitManager.DeleteUnit(RequestedUnit);
                break;
        }

        return Redirect("/Units");
    }

    public enum Actions
    {
        Delete
    }
}