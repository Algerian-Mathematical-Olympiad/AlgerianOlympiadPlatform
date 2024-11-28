using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages.Users;

[Authorize(Policy = "ViewUsers")]
public class UserModel : PageModel
{
    private readonly IMongoDatabase _database;
    
    [BindProperty(SupportsGet = true)]
    public required string RequestedUser { get; set; }

    [BindProperty] public DetailedUser UserInput { get; set; } = new();
    [BindProperty] public Actions Action { get; set; }

    private readonly IAuthorizationService _authorizationService;

    public UserModel(IMongoDatabase database, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _database = database;
    }
    
    public void OnGet()
    {
        if(RequestedUser != "new") 
        {
            UserInput = new UserManager(_database).GetUserDetails(RequestedUser);
        }
    }

    public async Task<IActionResult?> OnPostAsync()
    {

        switch (Action)
        {
            case Actions.Update:
                var result = await _authorizationService.AuthorizeAsync(User, "EditUsers");
                if(!result.Succeeded)
                {
                    return Redirect("/");
                }
                return RequestedUser == "new" ? Create() : Update();
            case Actions.Delete:
                var result1 = await _authorizationService.AuthorizeAsync(User, "DeleteUsers");
                if(!result1.Succeeded)
                {
                    return Redirect("/");
                }
                Delete();
                break;
        }

        return RedirectToPage("/Users/Index");
    }

    private void Delete()
    {
        var manager = new UserManager(_database);
        manager.DeleteUser(RequestedUser);
    }

    private IActionResult Create()
    {
        var manager = new UserManager(_database);
        if (IsUsernameUsed(manager)) throw new Exception("Username is already used");
        if (IsEmailUsed(manager)) throw new Exception("Email is already used");
        manager.CreateUser(UserInput);
        return RedirectToPage("/Users/Index");
    }

    private IActionResult? Update()
    {
        var manager = new UserManager(_database);

        if (IsUsernameUsed(manager)) throw new Exception("Username is already used");
        if (IsEmailUsed(manager)) throw new Exception("Email is already used");

        if (!ModelState.IsValid || UserInput.Id == "new") return Page();
        
        if (RequestedUser == UserInput.Id)
        {
            manager.UpdateUser(UserInput, RequestedUser);
            return null;
        }
        else
        {
            if (manager.DoesUserExist(UserInput.Id))
            {
                throw new Exception($"User with id {UserInput.Id} already exists");
            }
            if(RequestedUser != "new") manager.DeleteUser(RequestedUser);
            manager.CreateUser(UserInput);
            return Redirect("/Users/"+UserInput.Id);
        }
    }

    private bool IsUsernameUsed(UserManager userManager)
    {
        return UserInput.Id != RequestedUser && userManager.DoesUserExist(UserInput.Id);
    }

    private bool IsEmailUsed(UserManager userManager)
    {
        try
        {
            var user = userManager.GetUser(UserInput.Email);
            return user.Id != RequestedUser;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public enum Actions
    {
        Update,
        Delete
    }
    
}