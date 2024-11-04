using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;

namespace WebApplication1.Pages;

public class Login : PageModel
{
    private readonly ICustomerService _customerService;
    private readonly IConfiguration _configuration;

    public Login(ICustomerService customerService, IConfiguration configuration)
    {
        _customerService = customerService;
        _configuration = configuration;
    }
    
    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string Message { get; set; }

    public IActionResult OnGet()
    {
        if (CurrentUser.User.Role != null && CurrentUser.User.Role == "Admin"){
           
            return RedirectToPage("/Customers/Index");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        if (IsValidUser(Email, Password))
        {
            // Redirect to the desired page on successful login
            if (CurrentUser.User.Role == "Admin")
            {
                return RedirectToPage("/Customers/Index");
            }else if (CurrentUser.User.Role == "Customer")
            {
                return RedirectToPage("/MyProfile");
            }
        }
        Message = "Invalid username or password";
        return Page();
    }

    private bool IsValidUser(string username, string password)
    {
        var adminEmail = _configuration.GetValue("admin_account:email", "");
        var adminPassword = _configuration.GetValue("admin_account:password", "");
        if (username.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) && password.Equals(adminPassword))
        {
            CurrentUser.User.SetUser(adminEmail, "Admin");
            return true;
        }
        var user = _customerService.GetById(x => x.EmailAddress == username && x.Password == password);
        if (user != null)
        {
            CurrentUser.User.SetUser(user.EmailAddress, "Customer");
            return true;
        }
        else
        {
            return false;
        }
    }
}