using System.Security.Claims;
using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

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
        
        if (!user.IsStaff)
        {
            ErrorMessage = "User is not a staff member.";
            return Page();
        }

        // Sign in the user
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Id),
        };

        var permissions = new UserPermissionsManager(_database).GetUserPermissionsById(user.Id);

        if (permissions.IsSuperUser)
        {
            foreach (var item in Enum.GetNames(typeof(Permission)))
            {
                claims.Add(new Claim(item, item));
            }
        }
        else
        {
            foreach (var item in permissions.Permissions)
            {
                claims.Add(new Claim(item.ToString(), item.ToString()));
            }
        }

        var identity = new ClaimsIdentity(claims, "Cookie");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal);

        return RedirectToPage("/Index");
    }
}