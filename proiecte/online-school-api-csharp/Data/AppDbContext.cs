using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Books.Models;
using OnlineSchoolApi.Common.Validation;
using OnlineSchoolApi.Courses.Models;
using OnlineSchoolApi.Enrolments.Models;
using OnlineSchoolApi.Users.Administrators.Models;
using OnlineSchoolApi.Users.Models;
using OnlineSchoolApi.Users.Students.Models;
using OnlineSchoolApi.Users.Teachers.Models;

namespace OnlineSchoolApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Administrator> Administrators => Set<Administrator>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Enrolment> Enrolments => Set<Enrolment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        PrepareChanges();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        PrepareChanges();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    private void PrepareChanges()
    {
        var now = DateTime.Now;
        var entries = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified)
            .ToList();

        foreach (var entry in entries)
        {
            if (entry.Entity is User user)
            {
                if (entry.State == EntityState.Added)
                {
                    user.CreatedAt = now;
                }

                user.UpdatedAt = now;
            }

            EntityValidator.Validate(entry.Entity);
        }
    }
}
