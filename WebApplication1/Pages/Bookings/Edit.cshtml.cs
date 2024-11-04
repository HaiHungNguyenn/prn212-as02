using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Services.IService;

namespace WebApplication1.Pages.Bookings;

public class Edit : PageModel
{
    private readonly IBookingReservationService _bookingReservationService;
    private readonly CurrentUser _currentUser;

    [BindProperty]
    public BookingReservation? Booking { get; set; }

    public Edit(IBookingReservationService bookingReservationService)
    {
        _bookingReservationService = bookingReservationService;
        _currentUser = CurrentUser.User;
    }

    public IActionResult OnGet(int bookingId)
    {
        if (_currentUser.Name == null || _currentUser.Role != "Admin")
        {
            return RedirectToPage("/Login");
        }
        Booking = _bookingReservationService.GetAll()
            .Include(b => b.Customer)
            .FirstOrDefault(b => b.BookingReservationId == bookingId);

        if (Booking == null)
        {
            return NotFound();
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var bookingToUpdate = _bookingReservationService.GetById(b => b.BookingReservationId == Booking.BookingReservationId);

        if (bookingToUpdate == null)
        {
            return NotFound();
        }

        bookingToUpdate.BookingDate = Booking.BookingDate;
        bookingToUpdate.TotalPrice = Booking.TotalPrice;

        _bookingReservationService.Update(bookingToUpdate);

        return RedirectToPage("/Bookings/Index");
    }
}