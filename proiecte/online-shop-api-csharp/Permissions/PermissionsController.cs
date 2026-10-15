using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/permissions")]
public class PermissionsController(PermissionService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permissions.PermissionRead)]
    public Task<List<PermissionResponse>> GetAll(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);

    [HttpPost]
    [Authorize(Policy = Permissions.PermissionWrite)]
    public async Task<ActionResult<PermissionResponse>> Create(CreatePermissionRequest request, CancellationToken cancellationToken) =>
        StatusCode(StatusCodes.Status201Created, await service.CreateAsync(request, cancellationToken));

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.PermissionWrite)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
