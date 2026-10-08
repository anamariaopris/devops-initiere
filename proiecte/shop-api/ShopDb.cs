using Microsoft.EntityFrameworkCore;

namespace ShopApi;

public class ShopDb(DbContextOptions<ShopDb> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
}
