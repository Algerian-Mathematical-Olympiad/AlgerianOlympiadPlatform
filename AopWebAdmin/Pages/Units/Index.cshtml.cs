using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class UnitsModel : PageModel
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<UnitsModel> _logger;

    public List<Unit> AopUnits { get; set; }

    public UnitsModel(ILogger<UnitsModel> logger, IMongoDatabase database)
    {
        _logger = logger;
        _database = database;
    }

    public void OnGet()
    {
        GetUnits();
    }

    private void GetUnits()
    {
        var unitManager = new UnitManager(_database);
        AopUnits = unitManager.GetUnits();
    }

    [BindProperty]
    public string UnitToAffect { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                var unitManager = new UnitManager(_database);
                unitManager.DeleteUnit(UnitToAffect);
                break;
        }

        return Redirect("/Units");
    }

    public enum Actions
    {
        Delete
    }
}