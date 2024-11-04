using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Services.IService;

namespace WebApplication1.Pages.Bookings;

public class Detail : PageModel
{
    private readonly IBookingReservationService _bookingReservationService;

    public BookingReservation? Booking { get; set; }

    public Detail(IBookingReservationService bookingReservationService)
    {
        _bookingReservationService = bookingReservationService;
    }

    public IActionResult OnGet(int bookingId)
    {
        Booking = _bookingReservationService.GetAll()
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                .ThenInclude(d => d.Room)
            .FirstOrDefault(b => b.BookingReservationId == bookingId);

        if (Booking == null)
        {
            return NotFound();
        }

        return Page();
    }
}