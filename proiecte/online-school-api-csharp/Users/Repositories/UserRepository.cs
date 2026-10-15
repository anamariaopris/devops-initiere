using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Common.Extensions;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Users.Models;

namespace OnlineSchoolApi.Users.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public Task<User?> GetByEmailWithPermissionsAsync(string email, CancellationToken cancellationToken = default)
        => QueryByEmail(dbContext.Users.AsNoTracking(), email).FirstOrDefaultAsync(cancellationToken);

    public Task<User?> GetTrackedByEmailWithPermissionsAsync(string email, CancellationToken cancellationToken = default)
        => QueryByEmail(dbContext.Users, email).FirstOrDefaultAsync(cancellationToken);

    public async Task<User?> GetByIdWithPermissionsAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = id.ToEntityId();
        if (key is null)
        {
            return null;
        }

        return await dbContext.Users.AsNoTracking().Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Id == key.Value, cancellationToken);
    }

    public async Task<User?> GetTrackedByIdWithPermissionsAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = id.ToEntityId();
        if (key is null)
        {
            return null;
        }

        return await dbContext.Users.Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Id == key.Value, cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = email.ToLower();
        return dbContext.Users.AnyAsync(x => x.Email.ToLower() == normalized, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);

    private static IQueryable<User> QueryByEmail(IQueryable<User> source, string email)
    {
        var normalized = email.ToLower();
        return source.Include(x => x.Permissions).Where(x => x.Email.ToLower() == normalized);
    }
}
