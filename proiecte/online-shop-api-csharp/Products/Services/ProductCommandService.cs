using OnlineShopApi.Common;
using OnlineShopApi.Dtos;
using OnlineShopApi.Exceptions;
using OnlineShopApi.Repositories;

namespace OnlineShopApi.Services;

public class ProductCommandService(IProductRepository productRepository) : IProductCommandService
{
    public async Task<ProductsResponse> CreateProductAsync(ProductsDto dto, CancellationToken cancellationToken = default)
    {
        var product = dto.ToEntity();
        await productRepository.AddAsync(product, cancellationToken);
        await productRepository.SaveChangesAsync(cancellationToken);
        return product.ToResponse();
    }

    public async Task<ProductsResponse> UpdateProductAsync(Guid id, ProductsDto dto, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken) ?? throw new ProductDoesntExistException();
        product.Sku = dto.Sku;
        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Weight = dto.Weight;
        product.Descriptions = dto.Descriptions;
        product.Category = dto.Category;
        product.Stock = dto.Stock;
        if (dto.CreateDate is not null)
        {
            product.CreateDate = dto.CreateDate;
        }

        await productRepository.SaveChangesAsync(cancellationToken);
        return product.ToResponse();
    }

    public async Task DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken) ?? throw new ProductDoesntExistException();
        productRepository.Remove(product);
        await productRepository.SaveChangesAsync(cancellationToken);
    }
}
