using OnlineSchoolApi.Users.Administrators.Models;

namespace OnlineSchoolApi.Users.Administrators.Repositories;

public interface IAdministratorRepository
{
    Task<List<Administrator>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Administrator?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Administrator?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<Administrator>> SearchAsync(string term, CancellationToken cancellationToken = default);
}
