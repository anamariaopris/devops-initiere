using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/users")]
[Tags("Users")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.UserRead)]
    public async Task<ActionResult<List<UserResponse>>> List(CancellationToken cancellationToken)
        => Ok(await userService.FindAllAsync(cancellationToken));

    [HttpGet("{userId}")]
    [RequirePermission(Permissions.UserRead)]
    public async Task<ActionResult<UserResponse>> Get([FromRoute] Guid userId, CancellationToken cancellationToken)
        => Ok(await userService.FindByIdAsync(userId, cancellationToken));

    [HttpGet("{userId}/permissions")]
    [RequirePermission(Permissions.PermissionRead)]
    public async Task<ActionResult<List<string>>> ListPermissions([FromRoute] Guid userId, CancellationToken cancellationToken)
        => Ok(await userService.ListPermissionsAsync(userId, cancellationToken));

    [HttpPost("{userId}/permissions")]
    [RequirePermission(Permissions.PermissionWrite)]
    public async Task<ActionResult<List<string>>> Assign([FromRoute] Guid userId, [FromBody] AssignPermissionRequest request, CancellationToken cancellationToken)
        => Ok(await userService.AssignPermissionAsync(userId, request.Name!.Trim(), cancellationToken));

    [HttpDelete("{userId}/permissions/{permissionName}")]
    [RequirePermission(Permissions.PermissionWrite)]
    public async Task<ActionResult<List<string>>> Revoke([FromRoute] Guid userId, [FromRoute] string permissionName, CancellationToken cancellationToken)
        => Ok(await userService.RevokePermissionAsync(userId, permissionName, cancellationToken));
}
