using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/order-details")]
public class OrderDetailsController(OrderDetailService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permissions.UserRead)]
    public Task<OrderDetailsListResponse> GetAll(int page = 0, int size = 10, CancellationToken cancellationToken = default) =>
        service.GetPageAsync(page, size, cancellationToken);

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.UserRead)]
    public Task<OrderDetailsResponse> GetById(Guid id, CancellationToken cancellationToken) =>
        service.GetByIdAsync(id, cancellationToken);

    [HttpPost]
    [Authorize(Policy = Permissions.UserWrite)]
    public async Task<ActionResult<OrderDetailsResponse>> Create(OrderDetailsDto request, CancellationToken cancellationToken)
    {
        var detail = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = detail.Id }, detail);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.UserWrite)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
