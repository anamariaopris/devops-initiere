namespace OnlineShopApi.Models;

public class Order : AuditableEntity
{
    public Guid Id { get; set; }
    public int Ammount { get; set; }
    public string? ShippingAdress { get; set; }
    public string? OrderAdress { get; set; }
    public string? OrderEmail { get; set; }
    public DateOnly? OrderDate { get; set; }
    public string? OrderStatus { get; set; }
    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
