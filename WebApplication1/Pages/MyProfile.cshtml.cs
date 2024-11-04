using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Services.IService;

namespace WebApplication1.Pages;

public class MyProfile : PageModel
{
   
    private readonly ICustomerService _customerService;
    private readonly IBookingReservationService _bookingReservationService;
    private readonly CurrentUser _currentUser;

    [BindProperty]
    public Customer? User { get; set; }

    public ICollection<BookingReservation> BookingHistory { get; set; }

    public MyProfile(ICustomerService customerService, IBookingReservationService bookingReservationService)
    {
        _customerService = customerService;
        _bookingReservationService = bookingReservationService;
        _currentUser = CurrentUser.User;
    }

    public IActionResult OnGet()
     {
        var currentUser = _customerService.GetAll().FirstOrDefault(x => x.EmailAddress == _currentUser.Name);
        if (currentUser == null)
        {
            return RedirectToPage("/Login");
        }
        User = currentUser;
        BookingHistory = _bookingReservationService.GetAll()
            .Where(b => b.CustomerId == currentUser.CustomerId)
            .Include(x => x.BookingDetails)
            .ThenInclude(x => x.Room)
            .ToList();
        return Page();
    }

    public IActionResult OnPost()
    {
        // if (!ModelState.IsValid)
        // {
        //     return Page();
        // }
        //
        // var currentUser = _currentUser.User;
        // if (currentUser == null)
        // {
        //     return RedirectToPage("/Login");
        // }
        //
        // currentUser.CustomerFullName = User.CustomerFullName;
        // currentUser.EmailAddress = User.EmailAddress;
        // currentUser.Telephone = User.Telephone;
        // currentUser.CustomerBirthday = User.CustomerBirthday;
        // if (!string.IsNullOrEmpty(User.Password))
        // {
        //     currentUser.Password = User.Password; // Ensure to hash the password before saving
        // }
        //
        // _customerService.Update(currentUser);
        //
        return RedirectToPage();
    }
}


