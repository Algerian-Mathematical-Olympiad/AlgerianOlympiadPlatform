using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AopWebAdmin.Pages;

public class UserModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string RequestedUser { get; set; }
    [BindProperty] public DetailedUser UserInput { get; set; }
    [BindProperty] public Actions Action { get; set; }

    public void OnGet()
    {
        if(RequestedUser == "new") UserInput = new DetailedUser();
        else
        {
            UserInput = new UserManager(new TestDataBaseProvider().GetDatabase()).GetUserDetails(RequestedUser);
        }
    }

    public async Task<IActionResult?> OnPostAsync()
    {

        switch (Action)
        {
            case Actions.Update:
                if (RequestedUser == "new")
                {
                    return Create();
                }
                else
                {
                    Update();
                }
                break;
            case Actions.Delete:
                Delete();
                break;
        }

        return RedirectToPage("/Users/Index");
    }

    private void Delete()
    {
        var manager = new UserManager(new TestDataBaseProvider().GetDatabase());
        manager.DeleteUser(RequestedUser);
    }

    private IActionResult? Create()
    {
        var manager = new UserManager(new TestDataBaseProvider().GetDatabase());
        if (IsUsernameUsed(manager)) throw new Exception("Username is already used");
        if (IsEmailUsed(manager)) throw new Exception("Email is already used");
        manager.CreateUser(UserInput);
        return null;
    }

    private IActionResult? Update()
    {
        var manager = new UserManager(new TestDataBaseProvider().GetDatabase());

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