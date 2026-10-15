using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Common.Extensions;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Users.Students.Models;

namespace OnlineSchoolApi.Users.Students.Repositories;

public class StudentRepository(AppDbContext dbContext) : IStudentRepository
{
    public Task<List<Student>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Students.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task<Student?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = id.ToEntityId();
        return key is null
            ? null
            : await dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == key.Value, cancellationToken);
    }

    public async Task<Student?> GetTrackedByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = id.ToEntityId();
        return key is null
            ? null
            : await dbContext.Students.FirstOrDefaultAsync(x => x.Id == key.Value, cancellationToken);
    }

    public Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = email.ToLower();
        return dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Email.ToLower() == normalized, cancellationToken);
    }

    public Task<List<Student>> SearchAsync(string term, CancellationToken cancellationToken = default)
    {
        var normalized = term.ToLower();
        return dbContext.Students.AsNoTracking()
            .Where(x => x.FirstName.ToLower().Contains(normalized)
                        || x.LastName.ToLower().Contains(normalized)
                        || x.Email.ToLower().Contains(normalized))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = id.ToEntityId();
        return key is not null && await dbContext.Students.AnyAsync(x => x.Id == key.Value, cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(string lowerCaseEmail, CancellationToken cancellationToken = default)
        => dbContext.Students.AnyAsync(x => x.Email.ToLower() == lowerCaseEmail, cancellationToken);

    public async Task AddAsync(Student student, CancellationToken cancellationToken = default)
    {
        await dbContext.Students.AddAsync(student, cancellationToken);
    }

    public void Remove(Student student)
    {
        dbContext.Students.Remove(student);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
