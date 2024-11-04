namespace DAL;

public class BookingReservationDao : BaseDao<BookingReservation>
{
    public BookingReservationDao() : base(new FuminiHotelManagementContext())
    {
    }
    private static BookingReservationDao? _instance;
    public static BookingReservationDao Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BookingReservationDao();
            }
            return _instance;
        }
    }
}