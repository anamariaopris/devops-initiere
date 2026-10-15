using OnlineShopApi.Dtos;

namespace OnlineShopApi.Services;

public interface IProductQueryService
{
    Task<ProductsListResponse> FindAllProductsAsync(int page, int size, CancellationToken cancellationToken = default);
    Task<ProductsResponse> FindProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
