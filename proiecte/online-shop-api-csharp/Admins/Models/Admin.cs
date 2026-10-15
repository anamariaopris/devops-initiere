namespace OnlineShopApi.Models;

public class Admin : AuditableEntity
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Notes { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
