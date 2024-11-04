using System.Text.RegularExpressions;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;

namespace WebApplication1.Pages.Customers;

public class Create : PageModel
{
    private readonly ICustomerService _customerService;

    public Create(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [BindProperty]
    public Customer Customer { get; set; }

    public void OnGet()
    {
        Customer = new Customer();
    }

    public IActionResult OnPost()
    {
        const string phonePattern = @"^\+?\d{1,4}?[\s-.\(\)]?\(?\d{1,3}?\)?[\s-.\(\)]?\d{1,4}[\s-.\(\)]?\d{1,4}[\s-.\(\)]?\d{1,9}$";
        const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (string.IsNullOrEmpty(Customer.CustomerFullName))
                throw new Exception("Customer name can not be empty");

            if (string.IsNullOrEmpty(Customer.EmailAddress))
                throw new Exception("Email can not be empty");

            if (string.IsNullOrEmpty(Customer.Telephone))
                throw new Exception("Phone number cannot be empty");

            if (string.IsNullOrEmpty(Customer.Password))
                throw new Exception("Password can not be empty");

            if (!Regex.IsMatch(Customer.Telephone, phonePattern))
                throw new Exception("Phone number must have 10 digits only");

            if (!Regex.IsMatch(Customer.EmailAddress, emailPattern, RegexOptions.IgnoreCase))
                throw new Exception("Invalid email format");

            if (Customer.CustomerBirthday >= DateTime.Now)
                throw new Exception("Birth date must be before today");
        
            Customer.CustomerStatus = 0;
            var emailExists = _customerService.GetAll()
                .Any(x => x.EmailAddress == Customer.EmailAddress);
            if (emailExists)
                throw new Exception("Customer email already exists");
            
            _customerService.Create(Customer);
            return RedirectToPage("/Customers/Index");
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();
        }
    }
}