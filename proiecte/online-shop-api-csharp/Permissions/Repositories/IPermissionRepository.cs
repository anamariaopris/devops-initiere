using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public interface IPermissionRepository
{
    Task<List<Permission>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<Permission>> GetByNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default);
    Task AddAsync(Permission permission, CancellationToken cancellationToken = default);
    void Remove(Permission permission);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
