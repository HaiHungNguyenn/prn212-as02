using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;

namespace WebApplication1.Pages.Customers;

public class Index : PageModel
{
    private readonly ICustomerService _customerService;
    private readonly CurrentUser _currentUser;

    public Index(ICustomerService customerService)
    {
        _customerService = customerService;
        _currentUser = CurrentUser.User;
    }
    
    public ICollection<Customer> Customers { get; set; }
    
    public IActionResult OnGet()
    {
        if(_currentUser.Name == null || _currentUser.Role != "Admin")
        {
            return RedirectToPage("/Login");
        }
        Customers = _customerService.GetAll().ToList();
        return Page();
    }

    public IActionResult OnPostDelete(int customerId)
    {
        try
        {
            var customer = _customerService.GetById(x => x.CustomerId == customerId) ?? throw new Exception("Not found user");
            if (customer.BookingReservations.Any())
            {
                customer.CustomerStatus = 2;
                _customerService.Update(customer);
            }
            else
            {
                _customerService.Delete(customer);
            }
            return RedirectToPage();
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();
        }
    }
}