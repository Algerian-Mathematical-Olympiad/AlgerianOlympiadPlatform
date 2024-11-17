using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AopWebAdmin.Pages;

public class LogoutModel : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        // Sign out the user
        await HttpContext.SignOutAsync();
        return RedirectToPage("/Login");
    }
}