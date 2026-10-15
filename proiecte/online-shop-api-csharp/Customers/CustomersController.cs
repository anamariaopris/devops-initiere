using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController(CustomerService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permissions.UserRead)]
    public Task<CustomerListResponse> GetAll(int page = 0, int size = 10, CancellationToken cancellationToken = default) =>
        service.GetPageAsync(page, size, cancellationToken);

    [HttpGet("me")]
    public Task<CustomerResponse> GetMine(CancellationToken cancellationToken) =>
        service.GetByEmailAsync(User.GetEmail(), cancellationToken);

    [HttpPut("me")]
    public Task<CustomerResponse> UpdateMine(CustomerDto request, CancellationToken cancellationToken) =>
        service.UpdateByEmailAsync(User.GetEmail(), request, cancellationToken);

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        if (!User.HasPermission(Permissions.UserRead) && !await service.IsOwnerAsync(id, User.GetEmail(), cancellationToken))
        {
            return Forbid();
        }

        return await service.GetByIdAsync(id, cancellationToken);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.UserWrite)]
    public Task<CustomerResponse> Update(Guid id, CustomerDto request, CancellationToken cancellationToken) =>
        service.UpdateAsync(id, request, cancellationToken);

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.UserWrite)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
