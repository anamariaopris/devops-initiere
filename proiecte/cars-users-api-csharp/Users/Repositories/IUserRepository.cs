using OneToManyApi.Dtos;
using OneToManyApi.Models;

namespace OneToManyApi.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<User?> GetTrackedByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<User?> GetByIdWithCarsAsync(long id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<User>> SearchAsync(UserSearchRequest request, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    void Remove(User user);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
