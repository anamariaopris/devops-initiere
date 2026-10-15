using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/admins")]
public class AdminsController(AdminService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permissions.UserRead)]
    public Task<List<AdminResponse>> GetAll(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);

    [HttpGet("me")]
    public Task<AdminResponse> GetMine(CancellationToken cancellationToken) =>
        service.GetByEmailAsync(User.GetEmail(), cancellationToken);

    [HttpPut("me")]
    public Task<AdminResponse> UpdateMine(UpdateAdminRequest request, CancellationToken cancellationToken) =>
        service.UpdateByEmailAsync(User.GetEmail(), request, cancellationToken);

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.UserRead)]
    public Task<AdminResponse> GetById(Guid id, CancellationToken cancellationToken) =>
        service.GetByIdAsync(id, cancellationToken);

    [HttpPost]
    [Authorize(Policy = Permissions.UserWrite)]
    public async Task<ActionResult<AdminResponse>> Create(CreateAdminRequest request, CancellationToken cancellationToken)
    {
        var admin = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = admin.Id }, admin);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.UserWrite)]
    public Task<AdminResponse> Update(Guid id, UpdateAdminRequest request, CancellationToken cancellationToken) =>
        service.UpdateAsync(id, request, cancellationToken);

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.UserWrite)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
