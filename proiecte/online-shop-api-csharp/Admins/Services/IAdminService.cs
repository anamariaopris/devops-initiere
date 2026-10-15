using OnlineShopApi.Dtos;

namespace OnlineShopApi.Services;

public interface IAdminService
{
    Task<List<AdminResponse>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<AdminResponse> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AdminResponse> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<AdminResponse> CreateAsync(CreateAdminRequest request, CancellationToken cancellationToken = default);
    Task<AdminResponse> UpdateAsync(Guid id, UpdateAdminRequest request, CancellationToken cancellationToken = default);
    Task<AdminResponse> UpdateByEmailAsync(string email, UpdateAdminRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
