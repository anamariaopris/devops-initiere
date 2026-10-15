using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/order-details")]
[Tags("Order Details")]
public class OrderDetailsController(IOrderDetailQueryService orderDetailQueryService, IOrderDetailCommandService orderDetailCommandService) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.UserRead)]
    public async Task<ActionResult<OrderDetailsListResponse>> GetAllOrderDetails([FromQuery] int page = 0, [FromQuery] int size = 10, CancellationToken cancellationToken = default)
        => Ok(await orderDetailQueryService.FindAllOrderDetailsAsync(page, size, cancellationToken));

    [HttpGet("{id}")]
    [RequirePermission(Permissions.UserRead)]
    public async Task<ActionResult<OrderDetailsResponse>> GetOrderDetailById([FromRoute] Guid id, CancellationToken cancellationToken)
        => Ok(await orderDetailQueryService.FindOrderDetailByIdAsync(id, cancellationToken));

    [HttpPost]
    [RequirePermission(Permissions.UserWrite)]
    [ProducesResponseType(typeof(OrderDetailsResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrderDetailsResponse>> CreateOrderDetails([FromBody] OrderDetailsDto dto, CancellationToken cancellationToken)
        => StatusCode(StatusCodes.Status201Created, await orderDetailCommandService.CreateOrderDetailsAsync(dto, cancellationToken));

    [HttpDelete("{id}")]
    [RequirePermission(Permissions.UserWrite)]
    public async Task<IActionResult> DeleteOrderDetail([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await orderDetailCommandService.DeleteOrderDetailAsync(id, cancellationToken);
        return NoContent();
    }
}
