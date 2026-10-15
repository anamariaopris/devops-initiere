using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(UserService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permissions.UserRead)]
    public Task<List<UserResponse>> GetAll(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);

    [HttpGet("{userId}")]
    [Authorize(Policy = Permissions.UserRead)]
    public Task<UserResponse> GetById(Guid userId, CancellationToken cancellationToken) =>
        service.GetByIdAsync(userId, cancellationToken);

    [HttpGet("{userId}/permissions")]
    [Authorize(Policy = Permissions.PermissionRead)]
    public Task<List<string>> GetPermissions(Guid userId, CancellationToken cancellationToken) =>
        service.GetPermissionsAsync(userId, cancellationToken);

    [HttpPost("{userId}/permissions")]
    [Authorize(Policy = Permissions.PermissionWrite)]
    public Task<List<string>> AssignPermission(Guid userId, AssignPermissionRequest request, CancellationToken cancellationToken) =>
        service.AssignPermissionAsync(userId, request.Name.Trim(), cancellationToken);

    [HttpDelete("{userId}/permissions/{permissionName}")]
    [Authorize(Policy = Permissions.PermissionWrite)]
    public Task<List<string>> RevokePermission(Guid userId, string permissionName, CancellationToken cancellationToken) =>
        service.RevokePermissionAsync(userId, permissionName, cancellationToken);
}
