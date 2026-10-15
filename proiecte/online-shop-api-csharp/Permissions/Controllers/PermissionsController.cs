using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/permissions")]
[Tags("Permissions")]
public class PermissionsController(IPermissionService permissionService) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.PermissionRead)]
    public async Task<ActionResult<List<PermissionResponse>>> List(CancellationToken cancellationToken)
        => Ok(await permissionService.FindAllAsync(cancellationToken));

    [HttpPost]
    [RequirePermission(Permissions.PermissionWrite)]
    [ProducesResponseType(typeof(PermissionResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<PermissionResponse>> Create([FromBody] CreatePermissionRequest request, CancellationToken cancellationToken)
        => StatusCode(StatusCodes.Status201Created, await permissionService.CreateAsync(request, cancellationToken));

    [HttpDelete("{id}")]
    [RequirePermission(Permissions.PermissionWrite)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await permissionService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
