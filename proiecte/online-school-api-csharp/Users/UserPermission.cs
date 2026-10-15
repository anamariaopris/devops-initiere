namespace OnlineSchoolApi.Users;

public class UserPermission
{
    public int UserId { get; set; }
    public Permission Permission { get; set; }
    public User? User { get; set; }

    public static List<UserPermission> Grant(IEnumerable<Permission> permissions) =>
        permissions.Select(p => new UserPermission { Permission = p }).ToList();
}
