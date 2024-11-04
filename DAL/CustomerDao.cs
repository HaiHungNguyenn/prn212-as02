using BusinessObjects;

namespace DAL;

public class CustomerDao : BaseDao<Customer>
{
    private static CustomerDao? _instance;
    
    public CustomerDao() : base(new FuminiHotelManagementContext())
    {
    }
    
    public static CustomerDao Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CustomerDao();
            }
            return _instance;
        }
    }
}