using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Data;
using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public class PermissionRepository(AppDbContext dbContext) : IPermissionRepository
{
    public Task<List<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Permissions.AsNoTracking().ToListAsync(cancellationToken);

    public Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.Permissions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => dbContext.Permissions.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

    public Task<List<Permission>> GetByNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default)
    {
        var list = names.ToList();
        return dbContext.Permissions.Where(x => list.Contains(x.Name)).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Permission permission, CancellationToken cancellationToken = default)
        => await dbContext.Permissions.AddAsync(permission, cancellationToken);

    public void Remove(Permission permission) => dbContext.Permissions.Remove(permission);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
