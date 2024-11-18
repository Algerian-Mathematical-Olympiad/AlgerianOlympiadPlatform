using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebsite.Pages.Groups.Units;

public class UnitModel : PageModel
{
    private readonly IMongoDatabase _database;
    public Unit Unit { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string RequestedUnit { get; set; }
    [BindProperty(SupportsGet = true)]
    public string RequestedGroup { get; set; }
    public MathProblemset Problemset { get; set; }
    public List<Problem> Problems { get; set; }
    

    public UnitModel(IMongoDatabase db)
    {
        _database = db;
    }
    
    public void OnGet()
    {
        GetUnit();
    }

    private void GetUnit()
    {
        Unit = new UnitManager(_database).GetUnitById(RequestedUnit);
        Problemset = new ProblemsetManager(_database).GetProblemsetById(Unit.Problemset);
        Problemset.SetDatabase(_database);
        Problems = Problemset.GetProblems();
    }
}