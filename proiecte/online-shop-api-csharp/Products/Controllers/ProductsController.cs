using Microsoft.AspNetCore.Mvc;
using OnlineShopApi.Dtos;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Controllers;

[ApiController]
[Route("api/products")]
[Tags("Products")]
public class ProductsController(IProductQueryService productQueryService, IProductCommandService productCommandService) : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.ProductRead)]
    public async Task<ActionResult<ProductsListResponse>> GetAllProducts([FromQuery] int page = 0, [FromQuery] int size = 10, CancellationToken cancellationToken = default)
        => Ok(await productQueryService.FindAllProductsAsync(page, size, cancellationToken));

    [HttpGet("{id}")]
    [RequirePermission(Permissions.ProductRead)]
    public async Task<ActionResult<ProductsResponse>> GetProductById([FromRoute] Guid id, CancellationToken cancellationToken)
        => Ok(await productQueryService.FindProductByIdAsync(id, cancellationToken));

    [HttpPost]
    [RequirePermission(Permissions.ProductWrite)]
    [ProducesResponseType(typeof(ProductsResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<ProductsResponse>> CreateProduct([FromBody] ProductsDto dto, CancellationToken cancellationToken)
        => StatusCode(StatusCodes.Status201Created, await productCommandService.CreateProductAsync(dto, cancellationToken));

    [HttpPut("{id}")]
    [RequirePermission(Permissions.ProductWrite)]
    public async Task<ActionResult<ProductsResponse>> UpdateProduct([FromRoute] Guid id, [FromBody] ProductsDto dto, CancellationToken cancellationToken)
        => Ok(await productCommandService.UpdateProductAsync(id, dto, cancellationToken));

    [HttpDelete("{id}")]
    [RequirePermission(Permissions.ProductWrite)]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await productCommandService.DeleteProductAsync(id, cancellationToken);
        return NoContent();
    }
}
