using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Services.IService;

namespace WebApplication1.Pages.Rooms;

public class Create : PageModel
{
   
    private readonly IRoomInformationService _roomInformationService;
    private readonly IRoomTypeService _roomTypeService;

    [BindProperty]
    public RoomInformation RoomInformation { get; set; }
    [BindProperty]
    public ICollection<RoomType> RoomTypes { get; set; }

    public Create(IRoomInformationService roomInformationService, IRoomTypeService roomTypeService)
    {
        _roomInformationService = roomInformationService;
        _roomTypeService = roomTypeService;
    }

    public void OnGet()
    {
        RoomTypes = _roomTypeService.GetAll().ToList();
    }

    public IActionResult OnPost()
    {
        try
        {
            var invalidRoomId = _roomInformationService.GetAll().Any(x => x.RoomId == RoomInformation.RoomId);
            if (invalidRoomId)
                throw new Exception("Room ID is already exist.");
           

            if (RoomInformation.RoomMaxCapacity <= 0)
                throw new Exception("Room capacity must be greater than 0");

            if (RoomInformation.RoomPricePerDay < 0)
                throw new Exception("Room price must be greater than or equal to 0");

            if (string.IsNullOrEmpty(RoomInformation.RoomNumber)) {
                throw new Exception("Room number is required");
            }
            var existRoom = _roomInformationService.GetById(x => x.RoomNumber == RoomInformation.RoomNumber);
            if (existRoom is not null)
                throw new Exception($"The room number {RoomInformation.RoomNumber} is already exist");
            var existRoomType = _roomTypeService.GetById(x => x.RoomTypeId == RoomInformation.RoomTypeId) ?? throw new Exception("Not found room type");
            var newRoom = new RoomInformation
            {
                RoomNumber = RoomInformation.RoomNumber,
                RoomMaxCapacity = RoomInformation.RoomMaxCapacity,
                RoomDetailDescription = RoomInformation.RoomDetailDescription,
                RoomPricePerDay = RoomInformation.RoomPricePerDay,
                RoomStatus = byte.Parse("1"),   
                RoomTypeId = existRoomType.RoomTypeId
            };
            _roomInformationService.Create(newRoom);
            return RedirectToPage("/Rooms/Index");
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            RoomTypes = _roomTypeService.GetAll().ToList();
            return Page();
        }
    }
}