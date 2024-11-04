using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Services.IService;

namespace WebApplication1.Pages.Rooms;

public class Edit : PageModel
{
    private readonly IRoomInformationService _roomInformationService;
    private readonly IRoomTypeService _roomTypeService;
    private readonly CurrentUser _currentUser;

    public Edit(IRoomInformationService roomInformationService, IRoomTypeService roomTypeService)
    {
        _roomInformationService = roomInformationService;
        _roomTypeService = roomTypeService;
        _currentUser = CurrentUser.User;
    }

    [BindProperty]
    public RoomInformation RoomInformation { get; set; }
    [BindProperty]
    public ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();

    public IActionResult OnGet(int roomId)
    {
        if (_currentUser.Role != "Admin")
        {
            return RedirectToPage("/Login");
        }
        try
        {
            RoomInformation = _roomInformationService.GetById(x => x.RoomId == roomId) ?? throw new Exception("Room not found");
            RoomTypes = _roomTypeService.GetAll().ToList();
            return Page();
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();
        }
       
    }

    public IActionResult OnPost()
    {
        try
        {
            // if (!ModelState.IsValid)
            // {
            //     return Page();
            // }
            var existingRoom = _roomInformationService.GetById(x => x.RoomId == RoomInformation.RoomId) ?? throw new Exception("Room not found");;

            if (RoomInformation.RoomMaxCapacity <= 0)
                throw new Exception("Room capacity must be greater than 0");

            if (RoomInformation.RoomPricePerDay < 0)
                throw new Exception("Room price must be greater than or equal to 0");

            if (string.IsNullOrEmpty(RoomInformation.RoomNumber)) {
                throw new Exception("Room number is required");
            }
            
            var existRoom = _roomInformationService.GetAll().FirstOrDefault(x => x.RoomNumber == RoomInformation.RoomNumber);
            if(existRoom != null && existingRoom.RoomNumber != RoomInformation.RoomNumber)
            {
                throw new Exception("Room number is already exist. Please input another room number.");
            }

            var existRoomType = _roomTypeService.GetById(x => x.RoomTypeId == RoomInformation.RoomTypeId) ?? throw new Exception("Not Found Room Type");
            existingRoom.RoomNumber = RoomInformation.RoomNumber;
            existingRoom.RoomMaxCapacity = RoomInformation.RoomMaxCapacity;
            existingRoom.RoomDetailDescription = RoomInformation.RoomDetailDescription;
            existingRoom.RoomPricePerDay = RoomInformation.RoomPricePerDay;
            existingRoom.RoomTypeId = RoomInformation.RoomTypeId;
            existingRoom.RoomStatus = RoomInformation.RoomStatus;
            //existingRoom.RoomType = existRoomType;
            _roomInformationService.Update(existingRoom);
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