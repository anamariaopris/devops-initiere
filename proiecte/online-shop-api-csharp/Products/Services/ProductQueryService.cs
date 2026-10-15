using OnlineShopApi.Common;
using OnlineShopApi.Dtos;
using OnlineShopApi.Exceptions;
using OnlineShopApi.Repositories;

namespace OnlineShopApi.Services;

public class ProductQueryService(IProductRepository productRepository) : IProductQueryService
{
    public async Task<ProductsListResponse> FindAllProductsAsync(int page, int size, CancellationToken cancellationToken = default)
    {
        var result = await productRepository.GetPageAsync(page, size, cancellationToken);
        return new ProductsListResponse(
            result.Items.Select(p => p.ToResponse()).ToList(),
            result.TotalElements, result.TotalPages, result.PageNumber, result.PageSize);
    }

    public async Task<ProductsResponse> FindProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken) ?? throw new ProductDoesntExistException();
        return product.ToResponse();
    }
}
