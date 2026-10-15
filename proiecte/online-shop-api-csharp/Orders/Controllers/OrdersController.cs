using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/orders")]
[Tags("Orders")]
public class OrdersController(IOrderQueryService orderQueryService, IOrderCommandService orderCommandService) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.UserRead)]
    public async Task<ActionResult<OrdersListResponse>> GetAllOrders([FromQuery] int page = 0, [FromQuery] int size = 10, CancellationToken cancellationToken = default)
        => Ok(await orderQueryService.FindAllOrdersAsync(page, size, cancellationToken));

    [HttpGet("me")]
    [RequirePermission(Permissions.OrderRead)]
    public async Task<ActionResult<OrdersListResponse>> GetMyOrders([FromQuery] int page = 0, [FromQuery] int size = 10, CancellationToken cancellationToken = default)
        => Ok(await orderQueryService.FindOrdersByEmailAsync(User.GetEmail(), page, size, cancellationToken));

    [HttpPost("me")]
    [RequirePermission(Permissions.OrderWrite)]
    [ProducesResponseType(typeof(OrdersResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrdersResponse>> CreateMyOrder([FromBody] OrdersDto dto, CancellationToken cancellationToken)
        => StatusCode(StatusCodes.Status201Created, await orderCommandService.CreateOrderForCurrentUserAsync(User.GetEmail(), dto, cancellationToken));

    [HttpGet("{id}")]
    [RequirePermission(Permissions.UserRead, Owner = OwnedResource.Order, OwnerArgument = "id")]
    public async Task<ActionResult<OrdersResponse>> GetOrderById([FromRoute] Guid id, CancellationToken cancellationToken)
        => Ok(await orderQueryService.FindOrderByIdAsync(id, cancellationToken));

    [HttpGet("customer/{customerId}")]
    [RequirePermission(Permissions.UserRead, Owner = OwnedResource.Customer, OwnerArgument = "customerId")]
    public async Task<ActionResult<OrdersListResponse>> GetOrdersByCustomer([FromRoute] Guid customerId, [FromQuery] int page = 0, [FromQuery] int size = 10, CancellationToken cancellationToken = default)
        => Ok(await orderQueryService.FindOrdersByCustomerIdAsync(customerId, page, size, cancellationToken));

    [HttpPost]
    [RequirePermission(Permissions.UserWrite)]
    [ProducesResponseType(typeof(OrdersResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrdersResponse>> CreateOrder([FromQuery, BindRequired] Guid customerId, [FromBody] OrdersDto dto, CancellationToken cancellationToken)
        => StatusCode(StatusCodes.Status201Created, await orderCommandService.CreateOrderAsync(customerId, dto, cancellationToken));

    [HttpPatch("{id}/status")]
    [RequirePermission(Permissions.UserWrite)]
    public async Task<ActionResult<OrdersResponse>> UpdateOrderStatus([FromRoute] Guid id, [FromBody] Dictionary<string, string?> body, CancellationToken cancellationToken)
        => Ok(await orderCommandService.UpdateOrderStatusAsync(id, body.GetValueOrDefault("status"), cancellationToken));

    [HttpDelete("{id}")]
    [RequirePermission(Permissions.UserWrite)]
    public async Task<IActionResult> DeleteOrder([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await orderCommandService.DeleteOrderAsync(id, cancellationToken);
        return NoContent();
    }
}
