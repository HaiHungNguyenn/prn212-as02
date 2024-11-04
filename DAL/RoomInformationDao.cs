namespace DAL;

public class RoomInformationDao : BaseDao<RoomInformation>
{
    public RoomInformationDao() : base(new FuminiHotelManagementContext())
    {
    }
    private static RoomInformationDao? _instance;
    public static RoomInformationDao Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RoomInformationDao();
            }
            return _instance;
        }
    }
}