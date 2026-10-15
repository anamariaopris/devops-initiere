using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Errors;

namespace OnlineSchoolApi.Courses;

public class CourseService(AppDbContext db)
{
    public Task<List<CourseResponse>> GetAllAsync(CancellationToken cancellationToken) =>
        ListAsync(db.Courses, cancellationToken);

    public async Task<CourseResponse> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var course = await db.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw NotFound(id);
        return course.ToResponse();
    }

    public Task<List<CourseResponse>> FindByDepartmentAsync(string department, CancellationToken cancellationToken) =>
        ListAsync(db.Courses.Where(c => c.Department.Contains(department)), cancellationToken);

    public async Task<List<CourseResponse>> SearchAsync(string? query, CancellationToken cancellationToken)
    {
        var term = query?.Trim();
        if (string.IsNullOrEmpty(term))
        {
            return [];
        }
        return await ListAsync(db.Courses.Where(c => c.CourseName.Contains(term) || c.Department.Contains(term)), cancellationToken);
    }

    public async Task<CourseResponse> CreateAsync(CourseCreateRequest request, CancellationToken cancellationToken)
    {
        var course = new Course { CourseName = request.CourseName, Department = request.Departament };
        db.Courses.Add(course);
        await db.SaveChangesAsync(cancellationToken);
        return course.ToResponse();
    }

    public async Task<CourseResponse> UpdateAsync(long id, CourseUpdateRequest request, CancellationToken cancellationToken)
    {
        var course = await FindAsync(id, cancellationToken);
        if (request is { CourseName: null, Department: null })
        {
            throw BadRequestException.EmptyUpdate();
        }

        course.CourseName = request.CourseName ?? course.CourseName;
        course.Department = request.Department ?? course.Department;
        await db.SaveChangesAsync(cancellationToken);
        return course.ToResponse();
    }

    public async Task<CourseResponse> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var course = await FindAsync(id, cancellationToken);
        db.Courses.Remove(course);
        await db.SaveChangesAsync(cancellationToken);
        return course.ToResponse();
    }

    private static async Task<List<CourseResponse>> ListAsync(IQueryable<Course> query, CancellationToken cancellationToken)
    {
        var courses = await query.AsNoTracking().OrderBy(c => c.Id).ToListAsync(cancellationToken);
        return courses.Select(c => c.ToResponse()).ToList();
    }

    private async Task<Course> FindAsync(long id, CancellationToken cancellationToken) =>
        await db.Courses.FirstOrDefaultAsync(c => c.Id == id, cancellationToken) ?? throw NotFound(id);

    private static NotFoundException NotFound(long id) => new($"Course not found with id={id}");
}
