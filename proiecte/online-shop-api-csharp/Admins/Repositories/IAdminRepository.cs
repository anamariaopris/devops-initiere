using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public interface IAdminRepository
{
    Task<List<Admin>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Admin?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Admin?> GetByUserEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(Admin admin, CancellationToken cancellationToken = default);
    void Remove(Admin admin);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
