using OnlineShopApi.Dtos;

namespace OnlineShopApi.Services;

public interface IProductCommandService
{
    Task<ProductsResponse> CreateProductAsync(ProductsDto dto, CancellationToken cancellationToken = default);
    Task<ProductsResponse> UpdateProductAsync(Guid id, ProductsDto dto, CancellationToken cancellationToken = default);
    Task DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);
}
