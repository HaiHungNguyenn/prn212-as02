using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;

namespace WebApplication1.Pages.Customers;

public class Detail : PageModel
{
    private readonly ICustomerService _customerService;
    private readonly CurrentUser _currentUser;

    public Detail(ICustomerService customerService)
    {
        _customerService = customerService;
        _currentUser = CurrentUser.User;
    }
    
    public Customer? Customer { get; set; }
    public IActionResult OnGet(int customerId)
    {
        if(_currentUser.Role != "Admin")
        {
            RedirectToPage("/Login");
        }
        Customer = _customerService.GetById(x => x.CustomerId == customerId);
        return Page();
    }
}