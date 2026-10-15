namespace OnlineSchoolApi.Users.Models;

public class UserPermission
{
    public int UserId { get; set; }
    public Permission Permission { get; set; }
    public User? User { get; set; }
}
