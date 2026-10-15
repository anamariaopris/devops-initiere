using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Books;
using OnlineSchoolApi.Courses;
using OnlineSchoolApi.Enrolments;
using OnlineSchoolApi.Users;
using OnlineSchoolApi.Users.Administrators;
using OnlineSchoolApi.Users.Students;
using OnlineSchoolApi.Users.Teachers;

namespace OnlineSchoolApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Administrator> Administrators => Set<Administrator>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Enrolment> Enrolments => Set<Enrolment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<User>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
            }
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
