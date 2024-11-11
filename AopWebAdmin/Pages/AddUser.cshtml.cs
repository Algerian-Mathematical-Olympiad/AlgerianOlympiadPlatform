using DatabaseConnector;
using DatabaseConnector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AopWebAdmin.Pages;

public class AddUserModel : PageModel
{
    [BindProperty] public DetailedUserInputModel UserInput { get; set; } = new DetailedUserInputModel();

    public async Task<IActionResult> OnPostAsync()
    {
        var detailedUser = new DetailedUser(
            id: UserInput.Username,
            englishName: new UserName(UserInput.EnglishFirstName, UserInput.EnglishLastName),
            arabicName: new UserName(UserInput.ArabicFirstName, UserInput.ArabicLastName),
            birthday: UserInput.Birthday,
            email: UserInput.Email,
            school: new School(new Description(UserInput.SchoolName), UserInput.Wilaya, UserInput.SchoolType),
            schoolYear: UserInput.SchoolYear,
            tshirtSize: UserInput.TshirtSize,
            hasPassport: UserInput.HasPassport
        );

        var u = new UserManager(new TestDataBaseProvider().GetDatabase());


        if (IsUsernameUsed(u)) return RedirectToPage("Error", new { errorMessage = "Username already exists" });

        if (IsEmailUsed(u)) return RedirectToPage("Error", new { errorMessage = "Email already exists" });

        u.CreateUser(detailedUser);

        return RedirectToPage("Users");
    }

    private bool IsUsernameUsed(UserManager userManager)
    {
        try
        {
            userManager.GetUser(UserInput.Username);
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    private bool IsEmailUsed(UserManager userManager)
    {
        try
        {
            userManager.GetUser(UserInput.Email);
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public class DetailedUserInputModel
    {
        public string EnglishFirstName { get; set; }
        public string EnglishLastName { get; set; }
        public string ArabicFirstName { get; set; }
        public string ArabicLastName { get; set; }
        public DateOnly Birthday { get; set; }
        public string Email { get; set; }
        public string SchoolName { get; set; }
        public Wilaya Wilaya { get; set; }
        public SchoolType SchoolType { get; set; }
        public SchoolYear SchoolYear { get; set; }
        public TshirtSize TshirtSize { get; set; }
        public bool HasPassport { get; set; }
        public string Username { get; set; }
    }
}