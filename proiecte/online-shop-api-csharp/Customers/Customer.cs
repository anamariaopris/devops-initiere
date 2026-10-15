namespace OnlineShopApi.Models;

public class Customer : AuditableEntity
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;
    public string DefaultShippingAddress { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
