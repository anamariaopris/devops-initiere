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
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Brand).HasColumnName("brand").HasMaxLength(255);
            entity.Property(e => e.Model).HasColumnName("model").HasMaxLength(255);
            entity.Property(e => e.Color).HasColumnName("color").HasMaxLength(255);
            entity.Property(e => e.Size).HasColumnName("size");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Price).HasColumnName("price").HasPrecision(10, 2);
            entity.Property(e => e.Mileage).HasColumnName("mileage");
            entity.Property(e => e.FuelType).HasColumnName("fuel_type").HasMaxLength(255);
            entity.Property(e => e.Transmission).HasColumnName("transmission").HasMaxLength(255);
            entity.Property(e => e.Available).HasColumnName("available");
            entity.HasIndex(e => new { e.Brand, e.Model }).IsUnique();
        });
    }
}
