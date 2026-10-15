namespace OnlineShopApi.Models;

public class Product : AuditableEntity
{
    public Guid Id { get; set; }
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public int Price { get; set; }
    public int Weight { get; set; }
    public string? Descriptions { get; set; }
    public string? Category { get; set; }
    public DateTime? CreateDate { get; set; }
    public int Stock { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
