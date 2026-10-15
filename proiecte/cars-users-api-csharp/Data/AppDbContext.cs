using Microsoft.EntityFrameworkCore;
using OneToManyApi.Cars;
using OneToManyApi.Users;

namespace OneToManyApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Car> Cars => Set<Car>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255);
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasData(
                new User { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" });
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.ToTable("car");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Brand).HasColumnName("brand").HasMaxLength(255);
            entity.Property(e => e.Model).HasColumnName("model").HasMaxLength(255);
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.HasOne(e => e.User).WithMany(u => u.Cars).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasData(
                new Car { Id = 1, Brand = "Toyota", Model = "Corolla", Year = 2020, UserId = 1 },
                new Car { Id = 2, Brand = "Honda", Model = "Civic", Year = 2021, UserId = 1 },
                new Car { Id = 3, Brand = "Ford", Model = "Focus", Year = 2019, UserId = 2 });
        });
    }
}
