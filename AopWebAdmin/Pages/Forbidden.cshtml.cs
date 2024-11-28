using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AopWebAdmin.Pages;

public class ForbiddenModel : PageModel
{
    public void OnGet()
    {
    }

    public IActionResult GoHome()
    {
        return RedirectToPage("/");
    }
}   