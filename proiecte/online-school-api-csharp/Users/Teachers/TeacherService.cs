using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Errors;

namespace OnlineSchoolApi.Users.Teachers;

public class TeacherService(AppDbContext db)
{
    public Task<List<TeacherResponse>> GetAllAsync(CancellationToken cancellationToken) =>
        ListAsync(db.Teachers, cancellationToken);

    public async Task<TeacherResponse> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var teacher = await db.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken)
            ?? throw new NotFoundException($"teacher with id {id} not found");
        return teacher.ToResponse();
    }

    public async Task<TeacherResponse> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var teacher = await db.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.Email == email, cancellationToken)
            ?? throw new NotFoundException($"teacher with email {email} not found");
        return teacher.ToResponse();
    }

    public async Task<List<TeacherResponse>> SearchAsync(string? query, CancellationToken cancellationToken)
    {
        var term = query?.Trim();
        if (string.IsNullOrEmpty(term))
        {
            return [];
        }
        return await ListAsync(db.Teachers.Where(t =>
            t.FirstName.Contains(term) || t.LastName.Contains(term) || t.Email.Contains(term)), cancellationToken);
    }

    private static async Task<List<TeacherResponse>> ListAsync(IQueryable<Teacher> query, CancellationToken cancellationToken)
    {
        var teachers = await query.AsNoTracking().OrderBy(t => t.Id).ToListAsync(cancellationToken);
        return teachers.Select(t => t.ToResponse()).ToList();
    }
}
