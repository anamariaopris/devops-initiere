using OnlineSchoolApi.Users.Administrators.Dtos;

namespace OnlineSchoolApi.Users.Administrators.Services;

public interface IAdministratorQueryService
{
    Task<List<AdministratorResponse>> GetAllAdministratorsAsync(CancellationToken cancellationToken = default);
    Task<AdministratorResponse> GetAdministratorByIdAsync(long administratorId, CancellationToken cancellationToken = default);
    Task<AdministratorResponse> FindByEmailAsync(string? email, CancellationToken cancellationToken = default);
    Task<List<AdministratorResponse>> SearchAdministratorsAsync(string? query, CancellationToken cancellationToken = default);
}
