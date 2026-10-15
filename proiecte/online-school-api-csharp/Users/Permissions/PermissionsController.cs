using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineSchoolApi.Users.Permissions;

[ApiController]
public class PermissionsController(PermissionService service) : ControllerBase
{
    [HttpGet("permissions")]
    [Authorize(Policy = "permission:read")]
    public List<PermissionResponse> GetAll() => service.GetAll();

    [HttpGet("users/{userId}/permissions")]
    [Authorize(Policy = "user:read")]
    public Task<UserPermissionsResponse> GetForUser([Range(1, long.MaxValue)] long userId, CancellationToken cancellationToken) =>
        service.GetForUserAsync(userId, cancellationToken);

    [HttpPost("users/{userId}/permissions")]
    [Authorize(Policy = "user:write")]
    public Task<UserPermissionsResponse> Add([Range(1, long.MaxValue)] long userId, UpdateUserPermissionsRequest request, CancellationToken cancellationToken) =>
        service.AddAsync(userId, request.Permissions, cancellationToken);

    [HttpDelete("users/{userId}/permissions/{permission}")]
    [Authorize(Policy = "user:write")]
    public Task<UserPermissionsResponse> Remove([Range(1, long.MaxValue)] long userId, Permission permission, CancellationToken cancellationToken) =>
        service.RemoveAsync(userId, permission, cancellationToken);
}
