using OnlineSchoolApi.Common.Exceptions;
using OnlineSchoolApi.Users.Administrators.Dtos;
using OnlineSchoolApi.Users.Administrators.Exceptions;
using OnlineSchoolApi.Users.Administrators.Mappers;
using OnlineSchoolApi.Users.Administrators.Repositories;

namespace OnlineSchoolApi.Users.Administrators.Services;

public class AdministratorQueryService(IAdministratorRepository administratorRepository) : IAdministratorQueryService
{
    public async Task<List<AdministratorResponse>> GetAllAdministratorsAsync(CancellationToken cancellationToken = default)
        => (await administratorRepository.GetAllAsync(cancellationToken)).Select(AdministratorMapper.ToResponse).ToList();

    public async Task<AdministratorResponse> GetAdministratorByIdAsync(long administratorId, CancellationToken cancellationToken = default)
    {
        var administrator = await administratorRepository.GetByIdAsync(administratorId, cancellationToken)
            ?? throw new AdministratorNotFoundException(administratorId);

        return AdministratorMapper.ToResponse(administrator);
    }

    public async Task<AdministratorResponse> FindByEmailAsync(string? email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidRequestException("email must not be blank");
        }

        var administrator = await administratorRepository.GetByEmailAsync(email, cancellationToken)
            ?? throw new AdministratorNotFoundException(email);

        return AdministratorMapper.ToResponse(administrator);
    }

    public async Task<List<AdministratorResponse>> SearchAdministratorsAsync(string? query, CancellationToken cancellationToken = default)
    {
        var normalized = query?.Trim() ?? string.Empty;
        if (normalized.Length == 0)
        {
            return [];
        }

        return (await administratorRepository.SearchAsync(normalized, cancellationToken)).Select(AdministratorMapper.ToResponse).ToList();
    }
}
