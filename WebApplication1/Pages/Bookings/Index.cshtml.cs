using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Services.IService;

namespace WebApplication1.Pages.Bookings;

public class Index : PageModel
{
    private readonly IBookingReservationService _bookingReservationService;
    private readonly CurrentUser _currentUser;

    [BindProperty]
    public ICollection<BookingReservation> Bookings { get; set; } = new List<BookingReservation>();

    public Index(IBookingReservationService bookingReservationService)
    {
        _bookingReservationService = bookingReservationService;
        _currentUser = CurrentUser.User;
    }

    public IActionResult OnGet()
    {
        if (_currentUser.Name == null || _currentUser.Role != "Admin")
        {
            return RedirectToPage("/Login");
        }
        Bookings = _bookingReservationService.GetAll()
            .Include(x => x.Customer)
            .Include(x => x.BookingDetails)
            .ThenInclude(x => x.Room)
            .ToList();
        return Page();
    }

    public IActionResult OnPostDelete(int bookingId)
    {
        try
        {
            var booking = _bookingReservationService.GetAll()
                .Include(x => x.BookingDetails)
                .FirstOrDefault(x => x.BookingReservationId == bookingId) ?? throw new Exception("Booking not found");

            if (booking.BookingDetails.Any())
            {
                booking.BookingStatus = 2;
                _bookingReservationService.Update(booking);
            }
            else
            {
                _bookingReservationService.Delete(booking);
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