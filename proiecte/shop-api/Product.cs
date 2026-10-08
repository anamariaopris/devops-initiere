using Microsoft.EntityFrameworkCore;

namespace ShopApi;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    [Precision(10, 2)]
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
