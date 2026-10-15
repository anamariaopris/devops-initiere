namespace OnlineShopApi.Models;

public class Product : AuditableEntity
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Weight { get; set; }
    public string Descriptions { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public int Stock { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
