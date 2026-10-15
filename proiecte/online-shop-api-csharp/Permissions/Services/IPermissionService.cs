using OnlineShopApi.Dtos;

namespace OnlineShopApi.Services;

public interface IPermissionService
{
    Task<List<PermissionResponse>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<PermissionResponse> CreateAsync(CreatePermissionRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
