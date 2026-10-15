using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(ProductService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permissions.ProductRead)]
    public Task<ProductsListResponse> GetAll(int page = 0, int size = 10, CancellationToken cancellationToken = default) =>
        service.GetPageAsync(page, size, cancellationToken);

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.ProductRead)]
    public Task<ProductsResponse> GetById(Guid id, CancellationToken cancellationToken) =>
        service.GetByIdAsync(id, cancellationToken);

    [HttpPost]
    [Authorize(Policy = Permissions.ProductWrite)]
    public async Task<ActionResult<ProductsResponse>> Create(ProductsDto request, CancellationToken cancellationToken)
    {
        var product = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.ProductWrite)]
    public Task<ProductsResponse> Update(Guid id, ProductsDto request, CancellationToken cancellationToken) =>
        service.UpdateAsync(id, request, cancellationToken);

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.ProductWrite)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
