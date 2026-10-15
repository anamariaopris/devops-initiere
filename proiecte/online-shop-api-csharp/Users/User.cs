namespace OnlineShopApi.Models;

public class User : AuditableEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
