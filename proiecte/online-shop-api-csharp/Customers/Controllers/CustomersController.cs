using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/customers")]
[Tags("Customers")]
public class CustomersController(ICustomerQueryService customerQueryService, ICustomerCommandService customerCommandService) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.UserRead)]
    public async Task<ActionResult<CustomerListResponse>> GetAllCustomers([FromQuery] int page = 0, [FromQuery] int size = 10, CancellationToken cancellationToken = default)
        => Ok(await customerQueryService.FindAllCustomersAsync(page, size, cancellationToken));

    [HttpGet("me")]
    public async Task<ActionResult<CustomerResponse>> GetMyProfile(CancellationToken cancellationToken)
        => Ok(await customerQueryService.FindCustomerByEmailAsync(User.GetEmail(), cancellationToken));

    [HttpPut("me")]
    public async Task<ActionResult<CustomerResponse>> UpdateMyProfile([FromBody] CustomerDto dto, CancellationToken cancellationToken)
        => Ok(await customerCommandService.UpdateCustomerByEmailAsync(User.GetEmail(), dto, cancellationToken));

    [HttpGet("{id}")]
    [RequirePermission(Permissions.UserRead, Owner = OwnedResource.Customer, OwnerArgument = "id")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerById([FromRoute] Guid id, CancellationToken cancellationToken)
        => Ok(await customerQueryService.FindCustomerByIdAsync(id, cancellationToken));

    [HttpPut("{id}")]
    [RequirePermission(Permissions.UserWrite)]
    public async Task<ActionResult<CustomerResponse>> UpdateCustomer([FromRoute] Guid id, [FromBody] CustomerDto dto, CancellationToken cancellationToken)
        => Ok(await customerCommandService.UpdateCustomerAsync(id, dto, cancellationToken));

    [HttpDelete("{id}")]
    [RequirePermission(Permissions.UserWrite)]
    public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await customerCommandService.DeleteCustomerAsync(id, cancellationToken);
        return NoContent();
    }
}
