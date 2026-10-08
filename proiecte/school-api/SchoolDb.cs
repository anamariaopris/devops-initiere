using Microsoft.EntityFrameworkCore;

namespace SchoolApi;

public class SchoolDb(DbContextOptions<SchoolDb> options) : DbContext(options)
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
}
