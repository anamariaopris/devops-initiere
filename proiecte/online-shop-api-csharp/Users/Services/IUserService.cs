using OnlineShopApi.Dtos;

namespace OnlineShopApi.Services;

public interface IUserService
{
    Task<List<UserResponse>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<UserResponse> FindByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<List<string>> ListPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<List<string>> AssignPermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default);
    Task<List<string>> RevokePermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default);
}
