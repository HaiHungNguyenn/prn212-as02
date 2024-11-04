using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages;

public class Logout : PageModel
{
    private readonly CurrentUser _currentUser;

    public Logout()
    {
        _currentUser = CurrentUser.User;
    }
    public IActionResult OnGet()
    {
        _currentUser.ClearSession();
        return RedirectToPage("/Login");
    }
}