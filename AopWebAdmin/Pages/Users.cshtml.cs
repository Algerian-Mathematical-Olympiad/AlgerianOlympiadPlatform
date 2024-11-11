using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AopWebAdmin.Pages;

public class UsersModel : PageModel
{
    private readonly ILogger<UsersModel> _logger;

    public List<DetailedUser> AopUsers { get; set; }

    public UsersModel(ILogger<UsersModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        GetUsers();
    }

    private void GetUsers()
    {
        var u = new UserManager(new TestDataBaseProvider().GetDatabase());

        var users = u.GetUsersWithDetails();
        AopUsers = users;
    }
    
    [BindProperty]
    public string UserEmailToAffect { get; set; }
    [BindProperty]
    public Actions Action { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Delete:
                var u = new UserManager(new TestDataBaseProvider().GetDatabase());
                u.ThrowUser(UserEmailToAffect);
                break;
        }
        
        return RedirectToPage("Users"); 
    }


    public enum Actions
    {
        Delete
    }
}