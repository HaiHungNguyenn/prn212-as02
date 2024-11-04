using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;

namespace WebApplication1.Pages.Bookings;

public class Create : PageModel
{
    private readonly ICustomerService _customerService;
    private readonly IRoomInformationService _roomInformationService;
    private readonly IBookingReservationService _bookingReservationService;
    private readonly IBookingDetailService _bookingDetailService;
    private readonly CurrentUser _currentUser;

    [BindProperty]
    public int CustomerId { get; set; }
    [BindProperty]
    public int RoomId { get; set; }
    [BindProperty]
    public DateTime StartDate { get; set; }
    [BindProperty]
    public DateTime EndDate { get; set; }
    [BindProperty]
    public decimal TotalPrice { get; set; }

    public IEnumerable<Customer> Customers { get; set; }
    public IEnumerable<RoomInformation> Rooms { get; set; }

    public Create(ICustomerService customerService, IRoomInformationService roomInformationService, IBookingReservationService bookingReservationService, IBookingDetailService bookingDetailService)
    {
        _customerService = customerService;
        _roomInformationService = roomInformationService;
        _bookingReservationService = bookingReservationService;
        _bookingDetailService = bookingDetailService;
        _currentUser = CurrentUser.User;
    }

    public IActionResult OnGet()
    {
        if (_currentUser.Name == null || _currentUser.Role != "Admin")
        {
            return RedirectToPage("/Login");
        }
        Customers = _customerService.GetAll().ToList();
        Rooms = _roomInformationService.GetAll().ToList();
        return Page();
    }

    public IActionResult OnPost()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                Customers = _customerService.GetAll().ToList();
                Rooms = _roomInformationService.GetAll().ToList();
                return Page();
            }

            if (TotalPrice <= 0)
            {
                throw new Exception("Price must be greater than or equal to 0.");
            }

            if (StartDate < DateTime.Now || EndDate < DateTime.Now)
            {
                throw new Exception("The reservation date must be in the future.");
            }

            if (StartDate > EndDate)
            {
                throw new Exception("Start date must be before end date.");
            }

            var selectedRoom = _roomInformationService.GetById(x => x.RoomId == RoomId) ?? throw new Exception("Room does not exist");

            foreach (var bookingDetail in selectedRoom.BookingDetails)
            {
                if ((bookingDetail.StartDate <= StartDate && bookingDetail.EndDate >= StartDate) ||
                    (bookingDetail.StartDate <= EndDate && bookingDetail.EndDate >= EndDate))
                {
                    throw new Exception("Room is already booked in this period.");
                }
            }

            var customer = _customerService.GetById(x => x.CustomerId == CustomerId) ?? throw new Exception("Customer does not exist");

            var bookingReservation = new BookingReservation
            {
                BookingReservationId = GenerateBookingReservationId(),
                BookingDate = DateTime.Now,
                CustomerId = CustomerId,
                TotalPrice = TotalPrice,
                BookingStatus = 1
            };

            var newBookingDetail = new BookingDetail
            {
                StartDate = StartDate,
                EndDate = EndDate,
                ActualPrice = TotalPrice,
                RoomId = RoomId,
                BookingReservationId = bookingReservation.BookingReservationId
            };

            _bookingReservationService.Create(bookingReservation);
            _bookingDetailService.Create(newBookingDetail);
            return RedirectToPage("/Bookings/Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            Customers = _customerService.GetAll().ToList();
            Rooms = _roomInformationService.GetAll().ToList();
            return Page();
        }
    }

    private int GenerateBookingReservationId()
    {
        int randomInt;
        do
        {
            randomInt = new Random().Next();
        } while (_bookingReservationService.GetAll().Any(x => x.BookingReservationId == randomInt));

        return randomInt;
    }
}