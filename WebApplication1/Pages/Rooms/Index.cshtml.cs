using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Services.IService;

namespace WebApplication1.Pages.Rooms;

public class Index : PageModel
{
    private readonly IRoomInformationService _roomInformationService;
    private readonly CurrentUser _currentUser;

    [BindProperty] public ICollection<RoomInformation> Rooms { get; set; } = new List<RoomInformation>();  
    public Index(IRoomInformationService roomInformationService)
    {
        _roomInformationService = roomInformationService;
        _currentUser = CurrentUser.User;
    }

    public IActionResult OnGet()
    {
        if(_currentUser.Name == null || _currentUser.Role != "Admin")
        {
            return RedirectToPage("/Login");
        }
        Rooms = _roomInformationService.GetAll().Include(x => x.RoomType).ToList();
        return Page();
    }
    public IActionResult OnPostDelete(int roomId)
    {
        try
        {
            var room = _roomInformationService.GetAll().Include(x => x.BookingDetails).FirstOrDefault(x => x.RoomId == roomId) ?? throw new Exception("Not found room");
            if (room.BookingDetails.Any())
            {
                room.RoomStatus = 0;
                _roomInformationService.Update(room);
            }
            else
            {
                _roomInformationService.Delete(room);
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