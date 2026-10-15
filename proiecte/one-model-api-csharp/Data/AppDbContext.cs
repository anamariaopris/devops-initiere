using Microsoft.EntityFrameworkCore;
using OneModelApi.Models;

namespace OneModelApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Car> Cars => Set<Car>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.ToTable("car");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Brand).HasColumnName("brand").IsRequired();
            entity.Property(e => e.Model).HasColumnName("model").IsRequired();
            entity.Property(e => e.Color).HasColumnName("color").IsRequired();
            entity.Property(e => e.Size).HasColumnName("size").IsRequired();
            entity.Property(e => e.Year).HasColumnName("year").IsRequired();
            entity.Property(e => e.Price).HasColumnName("price").IsRequired();
            entity.Property(e => e.Mileage).HasColumnName("mileage").IsRequired();
            entity.Property(e => e.FuelType).HasColumnName("fuel_type").IsRequired();
            entity.Property(e => e.Transmission).HasColumnName("transmission").IsRequired();
            entity.Property(e => e.Available).HasColumnName("available").IsRequired();

            entity.HasIndex(e => new { e.Brand, e.Model }).IsUnique();
        });
    }
}
