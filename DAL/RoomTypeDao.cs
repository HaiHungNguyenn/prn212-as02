using BusinessObjects;

namespace DAL;

public class RoomTypeDao : BaseDao<RoomType>
{
    public RoomTypeDao() : base(new FuminiHotelManagementContext())
    {
    }
    private static RoomTypeDao? _instance;
    public static RoomTypeDao Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RoomTypeDao();
            }
            return _instance;
        }
    }
}