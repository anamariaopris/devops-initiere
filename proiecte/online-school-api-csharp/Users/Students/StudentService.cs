using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Courses;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Errors;

namespace OnlineSchoolApi.Users.Students;

public class StudentService(AppDbContext db)
{
    public Task<List<StudentResponse>> GetAllAsync(CancellationToken cancellationToken) =>
        ListAsync(db.Students, cancellationToken);

    public async Task<StudentResponse> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var student = await db.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken)
            ?? throw NotFound(id);
        return student.ToResponse();
    }

    public async Task<StudentResponse> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var student = await db.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Email == email, cancellationToken)
            ?? throw new NotFoundException($"Student not found with email={email}");
        return student.ToResponse();
    }

    public async Task<List<StudentResponse>> SearchAsync(string? query, CancellationToken cancellationToken)
    {
        var term = query?.Trim();
        if (string.IsNullOrEmpty(term))
        {
            return [];
        }
        return await ListAsync(db.Students.Where(s =>
            s.FirstName.Contains(term) || s.LastName.Contains(term) || s.Email.Contains(term)), cancellationToken);
    }

    public async Task<List<CourseResponse>> GetCoursesAsync(long id, CancellationToken cancellationToken)
    {
        if (!await db.Students.AnyAsync(s => s.Id == id, cancellationToken))
        {
            throw NotFound(id);
        }

        var courses = await db.Courses.AsNoTracking()
            .Where(c => c.Enrolments.Any(e => e.StudentId == id))
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);
        return courses.Select(c => c.ToResponse()).ToList();
    }

    public async Task<StudentResponse> CreateAsync(StudentCreateRequest request, CancellationToken cancellationToken)
    {
        await EnsureEmailIsFreeAsync(request.Email, cancellationToken);
        var student = new Student { FirstName = request.FirstName, LastName = request.LastName, Email = request.Email };
        db.Students.Add(student);
        await db.SaveChangesAsync(cancellationToken);
        return student.ToResponse();
    }

    public async Task<StudentResponse> UpdateAsync(long id, StudentUpdateRequest request, CancellationToken cancellationToken)
    {
        var student = await FindAsync(id, cancellationToken);
        if (request is { FirstName: null, LastName: null, Email: null })
        {
            throw BadRequestException.EmptyUpdate();
        }
        if (request.Email is not null && request.Email != student.Email)
        {
            await EnsureEmailIsFreeAsync(request.Email, cancellationToken);
        }

        student.FirstName = request.FirstName ?? student.FirstName;
        student.LastName = request.LastName ?? student.LastName;
        student.Email = request.Email ?? student.Email;
        await db.SaveChangesAsync(cancellationToken);
        return student.ToResponse();
    }

    public async Task<StudentResponse> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var student = await FindAsync(id, cancellationToken);
        db.Students.Remove(student);
        await db.SaveChangesAsync(cancellationToken);
        return student.ToResponse();
    }

    private static async Task<List<StudentResponse>> ListAsync(IQueryable<Student> query, CancellationToken cancellationToken)
    {
        var students = await query.AsNoTracking().OrderBy(s => s.Id).ToListAsync(cancellationToken);
        return students.Select(s => s.ToResponse()).ToList();
    }

    private async Task<Student> FindAsync(long id, CancellationToken cancellationToken) =>
        await db.Students.FirstOrDefaultAsync(s => s.Id == id, cancellationToken) ?? throw NotFound(id);

    private async Task EnsureEmailIsFreeAsync(string email, CancellationToken cancellationToken)
    {
        if (await db.Users.AnyAsync(u => u.Email == email, cancellationToken))
        {
            throw new ConflictException($"Student already exists with email={email}");
        }
    }

    private static NotFoundException NotFound(long id) => new($"Student not found with id={id}");
}
