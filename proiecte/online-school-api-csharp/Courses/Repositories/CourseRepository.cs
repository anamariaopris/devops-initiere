using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Common.Extensions;
using OnlineSchoolApi.Courses.Models;
using OnlineSchoolApi.Data;

namespace OnlineSchoolApi.Courses.Repositories;

public class CourseRepository(AppDbContext dbContext) : ICourseRepository
{
    public Task<List<Course>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Courses.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task<Course?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = id.ToEntityId();
        return key is null
            ? null
            : await dbContext.Courses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == key.Value, cancellationToken);
    }

    public async Task<Course?> GetTrackedByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = id.ToEntityId();
        return key is null
            ? null
            : await dbContext.Courses.FirstOrDefaultAsync(x => x.Id == key.Value, cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = id.ToEntityId();
        return key is not null && await dbContext.Courses.AnyAsync(x => x.Id == key.Value, cancellationToken);
    }

    public Task<List<Course>> GetByDepartmentContainingAsync(string department, CancellationToken cancellationToken = default)
    {
        var normalized = department.ToLower();
        return dbContext.Courses.AsNoTracking()
            .Where(x => x.Department.ToLower().Contains(normalized))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Course>> SearchAsync(string term, CancellationToken cancellationToken = default)
    {
        var normalized = term.ToLower();
        return dbContext.Courses.AsNoTracking()
            .Where(x => x.CourseName.ToLower().Contains(normalized) || x.Department.ToLower().Contains(normalized))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Course course, CancellationToken cancellationToken = default)
    {
        await dbContext.Courses.AddAsync(course, cancellationToken);
    }

    public void Remove(Course course)
    {
        dbContext.Courses.Remove(course);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
