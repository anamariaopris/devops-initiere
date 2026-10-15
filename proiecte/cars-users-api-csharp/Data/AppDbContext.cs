using Microsoft.EntityFrameworkCore;

namespace OneToManyApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Models.User> Users => Set<Models.User>();
    public DbSet<Models.Car> Cars => Set<Models.Car>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
