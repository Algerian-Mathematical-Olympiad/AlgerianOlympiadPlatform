using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Groups;

public class GroupDetails : PageModel
{
    private readonly IMongoDatabase _database;
    
    [BindProperty(SupportsGet = true)]
    public required string RequestedGroup { get; set; }

    [BindProperty(SupportsGet = false)] 
    public Group Group { get; set; } = new();
    
    [BindProperty(SupportsGet = false)]
    public Actions Action { get; set; }

    public List<UserPermissions> AvailableStaff { get; set; } = [];
    public List<UserPermissions> AvailableStudents { get; set; } = [];
    
    public List<IdOnly> AvailableUnits { get; set; } = [];
    private readonly IAuthorizationService _authorizationService;

    public GroupDetails(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _database = database;
    }
    
    public void OnGet()
    {
        GetGroup();
        FillAvailableUsers();
        FillAvailableUnits();
    }

    public async Task<IActionResult?> OnPost()
    {
        switch (Action)
        {
            case Actions.Update:
                var result = await _authorizationService.AuthorizeAsync(User, "UpdateGroups");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
                return Update();
            case Actions.Delete:
                var result1 = await _authorizationService.AuthorizeAsync(User, "DeleteGroups");
                if(!result1.Succeeded)
                {
                    return Redirect("/");
                }
                return Delete();
        }

        return null;
    }

    private IActionResult Delete()
    {
        new GroupManager(_database).DeleteGroup(RequestedGroup);
        return Redirect("/Groups");
    }

    private IActionResult? Update()
    {
        if (!ModelState.IsValid || Group.Id == "new") return Page();
        
        if (RequestedGroup == Group.Id)
        {
            new GroupManager(_database).UpdateGroup(Group, RequestedGroup);
            FillAvailableUsers();
            FillAvailableUnits();
            return null;
        }
        else
        {
            var manager = new GroupManager(_database);
            if (manager.GroupExists(Group.Id))
            {
                throw new Exception($"Group with id {Group.Id} already exists");
            }
            if(RequestedGroup != "new") manager.DeleteGroup(RequestedGroup);
            manager.CreateGroup(Group);
            return Redirect("/Groups/"+Group.Id);
        }
    }

    private void GetGroup()
    {
        if (RequestedGroup != "new")
        {
            Group = new GroupManager(_database).GetGroupById(RequestedGroup);
        }
    }
    
    private void FillAvailableUsers()
    {
        var permissionsManager = new UserPermissionsManager(_database);
        AvailableStudents = permissionsManager.GetStudents().Where(x => !Group.Students.Contains(x.Id)).ToList();
        AvailableStaff = permissionsManager.GetStaff().Where(x => !Group.Coaches.Contains(x.Id)).ToList();

    }

    private void FillAvailableUnits()
    {
        var manager = new UnitManager(_database);
        AvailableUnits = manager.GetUnitsIds().Where(x => !Group.Units.Contains(x.Id)).ToList();
    }

    public enum Actions
    {
        Delete,
        Update
    }
}