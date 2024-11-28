using System.ComponentModel.DataAnnotations;
using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebsite.Pages;
[AllowAnonymous]
public class Register : PageModel
{
    [BindProperty]
    public DetailedUser UserInput { get; set; } = new();
    [BindProperty]
    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = string.Empty;
    [BindProperty]
    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    private readonly IMongoDatabase _database;

    public Register(IMongoDatabase db)
    {
        _database = db;
    }

    
    public void OnGet()
    {
        
    }
    
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var manager = new UserManager(_database);
        // Check if the user already exists in the database
        if (manager.DoesUserExist(UserInput.Id))
        {
            ModelState.AddModelError(string.Empty, "Username is already taken.");
            return Page();
        }

        if (manager.DoesUserExist(UserInput.Email))
        {
            ModelState.AddModelError(string.Empty, "Email is already registered.");
            return Page();
        }

        // Save the new user to the database
        manager.CreateUser(UserInput);
        manager.ChangePassword(new UserAuthInfo()
        {
            Email = UserInput.Email,
            Id = UserInput.Id,
            PasswordHash = PasswordHasher.HashPassword(Password),
            IsStaff = false
        });
        
        return RedirectToPage("/Login");
    }


}