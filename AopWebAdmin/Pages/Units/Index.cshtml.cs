using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Units;

[Authorize(Policy = "ViewUnits")]
public class UnitsModel : PageModel
{
    private readonly IMongoDatabase _database;

    public List<Unit> Units { get; set; } = [];
    
    [BindProperty]
    public required string RequestedUnit { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    private readonly IAuthorizationService _authorizationService;

    public UnitsModel(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
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
    
    public async Task<IActionResult> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                var result = await _authorizationService.AuthorizeAsync(User, "DeleteUnits");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
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