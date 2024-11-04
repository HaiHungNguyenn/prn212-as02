namespace BusinessObjects;

public class CurrentUser
{
    private static CurrentUser? _currentUser;
    public static CurrentUser User => _currentUser ??= new CurrentUser();
    
    public string? Name { get; set; }
    public string? Role { get; set; }

    public void SetUser( string name, string role)
    {
        Name = name;
        Role = role;
    }

    public void ClearSession()
    {
        Name = null;
        Role = null;
    }
    
    public static bool IsAuthenticated => User.Name != null;
    public static bool IsAdmin => User.Role == "Admin";
    public static bool IsCustomer => User.Role == "Customer";
}