using Microsoft.AspNetCore.Mvc;
using OnlineSchoolApi.Common.Exceptions;
using OnlineSchoolApi.Common.Validation;
using OnlineSchoolApi.Users.Models;
using OnlineSchoolApi.Users.Permissions.Dtos;
using OnlineSchoolApi.Users.Permissions.Services;

namespace OnlineSchoolApi.Users.Permissions.Controllers;

[ApiController]
[Route("users/{userId}/permissions")]
[Tags("User Permissions")]
public class UserPermissionsController(IPermissionService permissionService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get permissions for user")]
    [EndpointDescription("Required permission: user:read")]
    public async Task<ActionResult<UserPermissionsResponse>> GetUserPermissions([FromRoute, Positive] long userId, CancellationToken cancellationToken)
        => Ok(await permissionService.GetUserPermissionsAsync(userId, cancellationToken));

    [HttpPost]
    [EndpointSummary("Add permissions to user")]
    [EndpointDescription("Required permission: user:write")]
    public async Task<ActionResult<UserPermissionsResponse>> AddPermissions([FromRoute, Positive] long userId, [FromBody] UpdateUserPermissionsRequest request, CancellationToken cancellationToken)
        => Ok(await permissionService.AddPermissionsAsync(userId, request.Permissions ?? [], cancellationToken));

    [HttpDelete("{permission}")]
    [EndpointSummary("Remove permission from user")]
    [EndpointDescription("Required permission: user:write")]
    public async Task<ActionResult<UserPermissionsResponse>> RemovePermission([FromRoute, Positive] long userId, [FromRoute] string permission, CancellationToken cancellationToken)
        => Ok(await permissionService.RemovePermissionAsync(userId, ParsePermission(permission), cancellationToken));

    private static Permission ParsePermission(string value)
    {
        var name = value.Trim();
        if (!Enum.GetNames<Permission>().Contains(name, StringComparer.Ordinal))
        {
            throw new TypeMismatchException("permission");
        }

        return Enum.Parse<Permission>(name);
    }
}
