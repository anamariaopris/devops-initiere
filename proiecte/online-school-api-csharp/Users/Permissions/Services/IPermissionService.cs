using OnlineSchoolApi.Users.Models;
using OnlineSchoolApi.Users.Permissions.Dtos;

namespace OnlineSchoolApi.Users.Permissions.Services;

public interface IPermissionService
{
    List<PermissionResponse> GetAllPermissions();
    Task<UserPermissionsResponse> GetUserPermissionsAsync(long userId, CancellationToken cancellationToken = default);
    Task<UserPermissionsResponse> AddPermissionsAsync(long userId, IEnumerable<Permission> permissions, CancellationToken cancellationToken = default);
    Task<UserPermissionsResponse> RemovePermissionAsync(long userId, Permission permission, CancellationToken cancellationToken = default);
}
