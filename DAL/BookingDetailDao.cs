using BusinessObjects;

namespace DAL;

public class BookingDetailDao : BaseDao<BookingDetail>
{
    public BookingDetailDao() : base(new FuminiHotelManagementContext())
    {
    }
    private static BookingDetailDao? _instance;
    public static BookingDetailDao Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BookingDetailDao();
            }
            return _instance;
        }
    }
}