using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/admins")]
[Tags("Admins")]
public class AdminsController(IAdminService adminService) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.UserRead)]
    public async Task<ActionResult<List<AdminResponse>>> List(CancellationToken cancellationToken)
        => Ok(await adminService.FindAllAsync(cancellationToken));

    [HttpGet("me")]
    public async Task<ActionResult<AdminResponse>> Me(CancellationToken cancellationToken)
        => Ok(await adminService.FindByEmailAsync(User.GetEmail(), cancellationToken));

    [HttpPut("me")]
    public async Task<ActionResult<AdminResponse>> UpdateMe([FromBody] UpdateAdminRequest request, CancellationToken cancellationToken)
        => Ok(await adminService.UpdateByEmailAsync(User.GetEmail(), request, cancellationToken));

    [HttpGet("{id}")]
    [RequirePermission(Permissions.UserRead)]
    public async Task<ActionResult<AdminResponse>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        => Ok(await adminService.FindByIdAsync(id, cancellationToken));

    [HttpPost]
    [RequirePermission(Permissions.UserWrite)]
    [ProducesResponseType(typeof(AdminResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<AdminResponse>> Create([FromBody] CreateAdminRequest request, CancellationToken cancellationToken)
        => StatusCode(StatusCodes.Status201Created, await adminService.CreateAsync(request, cancellationToken));

    [HttpPut("{id}")]
    [RequirePermission(Permissions.UserWrite)]
    public async Task<ActionResult<AdminResponse>> Update([FromRoute] Guid id, [FromBody] UpdateAdminRequest request, CancellationToken cancellationToken)
        => Ok(await adminService.UpdateAsync(id, request, cancellationToken));

    [HttpDelete("{id}")]
    [RequirePermission(Permissions.UserWrite)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await adminService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
