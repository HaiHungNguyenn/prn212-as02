using System.Text.RegularExpressions;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;

namespace WebApplication1.Pages.Customers;

public class Edit : PageModel
{
    public const string PhonePattern = @"^(\d{10})$";
    public const string EmailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    private readonly ICustomerService _customerService;
    private readonly CurrentUser _currentUser;

    public Edit(ICustomerService customerService)
    {
        _customerService = customerService;
        _currentUser = CurrentUser.User;
    }
    
    [BindProperty]
    public Customer? Customer { get; set; }

    [BindProperty] public string Message { get; set; } = string.Empty;
    public IActionResult OnGet(int customerId)
    {
        if (_currentUser.Role != "Admin")
        {
            return RedirectToPage("/Login");
        }
        Customer = _customerService.GetById(x => x.CustomerId == customerId);
        return Page();
    }

    public IActionResult OnPost(Customer customer)
    {
        try
        {
            // if (!ModelState.IsValid)
            // {
            //     Message = "Invalid data";
            //     return RedirectToPage("/Customers/Edit", new { customerId = Customer!.CustomerId });
            // }

            if (string.IsNullOrEmpty(customer.CustomerFullName))
                throw new Exception("Customer name is required");

            if (!Regex.IsMatch(customer.EmailAddress, EmailPattern))
                throw new Exception("Invalid email format");

            if (!Regex.IsMatch(customer.Telephone, PhonePattern))
                throw new Exception("Invalid phone format");
            var existingCustomer = _customerService.GetById(x => x.CustomerId == customer.CustomerId);
            if (existingCustomer == null)
                throw new Exception("Customer not found");

            existingCustomer.CustomerFullName = Customer.CustomerFullName;
            existingCustomer.EmailAddress = Customer.EmailAddress;
            existingCustomer.Telephone = Customer.Telephone;
            existingCustomer.CustomerBirthday = Customer.CustomerBirthday;
            existingCustomer.Password = Customer.Password;
            _customerService.Update(existingCustomer);
            Message = "Customer updated successfully";
            return RedirectToPage("/Customers/Index");
        }
        catch (Exception e)
        {
            // Message = e.Message;
            // // Customer = _customerService.GetById(x => x.CustomerId == customer.CustomerId);
            // return RedirectToPage();
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();

        }
    }
}