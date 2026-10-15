using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Common.Extensions;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Users.Teachers.Models;

namespace OnlineSchoolApi.Users.Teachers.Repositories;

public class TeacherRepository(AppDbContext dbContext) : ITeacherRepository
{
    public Task<List<Teacher>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Teachers.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task<Teacher?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = id.ToEntityId();
        return key is null
            ? null
            : await dbContext.Teachers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == key.Value, cancellationToken);
    }

    public Task<Teacher?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = email.ToLower();
        return dbContext.Teachers.AsNoTracking().FirstOrDefaultAsync(x => x.Email.ToLower() == normalized, cancellationToken);
    }

    public Task<List<Teacher>> SearchAsync(string term, CancellationToken cancellationToken = default)
    {
        var normalized = term.ToLower();
        return dbContext.Teachers.AsNoTracking()
            .Where(x => x.FirstName.ToLower().Contains(normalized)
                        || x.LastName.ToLower().Contains(normalized)
                        || x.Email.ToLower().Contains(normalized))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }
}
