using System.Security.Claims;
using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebsite.Pages;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly IMongoDatabase _database;

    public LoginModel(IMongoDatabase database)
    {
        _database = database;
    }

    [BindProperty] public required InputModel Input { get; set; }

    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
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