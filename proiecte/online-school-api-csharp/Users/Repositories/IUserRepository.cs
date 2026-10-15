using OnlineSchoolApi.Users.Models;

namespace OnlineSchoolApi.Users.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailWithPermissionsAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByIdWithPermissionsAsync(long id, CancellationToken cancellationToken = default);
    Task<User?> GetTrackedByEmailWithPermissionsAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetTrackedByIdWithPermissionsAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
