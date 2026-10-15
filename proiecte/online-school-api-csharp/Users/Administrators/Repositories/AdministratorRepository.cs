using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Common.Extensions;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Users.Administrators.Models;

namespace OnlineSchoolApi.Users.Administrators.Repositories;

public class AdministratorRepository(AppDbContext dbContext) : IAdministratorRepository
{
    public Task<List<Administrator>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Administrators.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task<Administrator?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = id.ToEntityId();
        return key is null
            ? null
            : await dbContext.Administrators.AsNoTracking().FirstOrDefaultAsync(x => x.Id == key.Value, cancellationToken);
    }

    public Task<Administrator?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = email.ToLower();
        return dbContext.Administrators.AsNoTracking().FirstOrDefaultAsync(x => x.Email.ToLower() == normalized, cancellationToken);
    }

    public Task<List<Administrator>> SearchAsync(string term, CancellationToken cancellationToken = default)
    {
        var normalized = term.ToLower();
        return dbContext.Administrators.AsNoTracking()
            .Where(x => x.FirstName.ToLower().Contains(normalized)
                        || x.LastName.ToLower().Contains(normalized)
                        || x.Email.ToLower().Contains(normalized))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }
}
