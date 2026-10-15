using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(OrderService service, CustomerService customers) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permissions.UserRead)]
    public Task<OrdersListResponse> GetAll(int page = 0, int size = 10, CancellationToken cancellationToken = default) =>
        service.GetPageAsync(page, size, cancellationToken);

    [HttpGet("me")]
    [Authorize(Policy = Permissions.OrderRead)]
    public Task<OrdersListResponse> GetMine(int page = 0, int size = 10, CancellationToken cancellationToken = default) =>
        service.GetMineAsync(User.GetEmail(), page, size, cancellationToken);

    [HttpPost("me")]
    [Authorize(Policy = Permissions.OrderWrite)]
    public async Task<ActionResult<OrdersResponse>> CreateMine(OrdersDto request, CancellationToken cancellationToken)
    {
        var order = await service.CreateMineAsync(User.GetEmail(), request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrdersResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        if (!User.HasPermission(Permissions.UserRead) && !await service.IsOwnerAsync(id, User.GetEmail(), cancellationToken))
        {
            return Forbid();
        }

        return await service.GetByIdAsync(id, cancellationToken);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<OrdersListResponse>> GetByCustomer(
        Guid customerId, int page = 0, int size = 10, CancellationToken cancellationToken = default)
    {
        if (!User.HasPermission(Permissions.UserRead) && !await customers.IsOwnerAsync(customerId, User.GetEmail(), cancellationToken))
        {
            return Forbid();
        }

        return await service.GetByCustomerAsync(customerId, page, size, cancellationToken);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.UserWrite)]
    public async Task<ActionResult<OrdersResponse>> Create([BindRequired] Guid customerId, OrdersDto request, CancellationToken cancellationToken)
    {
        var order = await service.CreateAsync(customerId, request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Policy = Permissions.UserWrite)]
    public Task<OrdersResponse> UpdateStatus(Guid id, OrderStatusRequest request, CancellationToken cancellationToken) =>
        service.UpdateStatusAsync(id, request.Status, cancellationToken);

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.UserWrite)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
