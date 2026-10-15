namespace OnlineShopApi.Models;

public class Order : AuditableEntity
{
    public Guid Id { get; set; }
    public decimal Ammount { get; set; }
    public string ShippingAdress { get; set; } = string.Empty;
    public string OrderAdress { get; set; } = string.Empty;
    public string OrderEmail { get; set; } = string.Empty;
    public DateOnly OrderDate { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
