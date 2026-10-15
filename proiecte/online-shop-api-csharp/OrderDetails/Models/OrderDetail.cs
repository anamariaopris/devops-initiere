namespace OnlineShopApi.Models;

public class OrderDetail : AuditableEntity
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public string? Sku { get; set; }
    public int Quantity { get; set; }
    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }
}
