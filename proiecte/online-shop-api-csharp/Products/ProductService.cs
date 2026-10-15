using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Common;
using OnlineShopApi.Data;
using OnlineShopApi.Dtos;
using OnlineShopApi.Errors;
using OnlineShopApi.Models;

namespace OnlineShopApi.Services;

public class ProductService(AppDbContext db)
{
    public async Task<ProductsListResponse> GetPageAsync(int page, int size, CancellationToken cancellationToken)
    {
        var result = await db.Products.AsNoTracking().OrderBy(p => p.Id).ToPageAsync(page, size, true, cancellationToken);
        return new ProductsListResponse(
            result.Items.Select(p => p.ToResponse()).ToList(),
            result.TotalElements, result.TotalPages, result.PageNumber, result.PageSize);
    }

    public async Task<ProductsResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        (await FindAsync(id, cancellationToken)).ToResponse();

    public async Task<ProductsResponse> CreateAsync(ProductsDto request, CancellationToken cancellationToken)
    {
        var product = new Product { CreateDate = request.CreateDate ?? DateTime.UtcNow };
        Copy(request, product);
        db.Products.Add(product);
        await db.SaveChangesAsync(cancellationToken);
        return product.ToResponse();
    }

    public async Task<ProductsResponse> UpdateAsync(Guid id, ProductsDto request, CancellationToken cancellationToken)
    {
        var product = await FindAsync(id, cancellationToken);
        Copy(request, product);
        product.CreateDate = request.CreateDate ?? product.CreateDate;
        await db.SaveChangesAsync(cancellationToken);
        return product.ToResponse();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        db.Products.Remove(await FindAsync(id, cancellationToken));
        await db.SaveChangesAsync(cancellationToken);
    }

    private async Task<Product> FindAsync(Guid id, CancellationToken cancellationToken) =>
        await db.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
        ?? throw new NotFoundException("Product does not exist in the database.");

    private static void Copy(ProductsDto request, Product product)
    {
        product.Sku = request.Sku.Trim();
        product.Name = request.Name.Trim();
        product.Price = request.Price;
        product.Weight = request.Weight;
        product.Descriptions = request.Descriptions.Trim();
        product.Category = request.Category.Trim();
        product.Stock = request.Stock;
    }
}
