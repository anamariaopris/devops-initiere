namespace OnlineShopApi.Models;

public class Customer : AuditableEntity
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? BillingAddress { get; set; }
    public string? DefaultShippingAddress { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
