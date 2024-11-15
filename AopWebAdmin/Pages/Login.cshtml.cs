using System.Security.Claims;
using AopWebAdmin;
using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly IMongoDatabase _database;

    public LoginModel(IMongoDatabase database)
    {
        _database = database;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ErrorMessage { get; set; }

    public class InputModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        UserAuthInfo user;

        try
        {
            user = new UserManager(_database).GetUserAuthInfo(Input.Email);
        }
        catch
        {
            ErrorMessage = "Invalid email or password.";
            return Page();
        }

        if (!PasswordHasher.VerifyPassword(user.PasswordHash, Input.Password))
        {
            ErrorMessage = "Invalid email or password.";
            return Page();
        }
        
        if (!user.IsSuperUser)
        {
            ErrorMessage = "User is not a superuser.";
            return Page();
        }

        // Sign in the user
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Id),
        };
        var identity = new ClaimsIdentity(claims, "Cookie");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal);

        return RedirectToPage("/Index");
    }
}