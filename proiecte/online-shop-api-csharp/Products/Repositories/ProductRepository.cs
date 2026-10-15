using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Common;
using OnlineShopApi.Data;
using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    public Task<PageResult<Product>> GetPageAsync(int page, int size, CancellationToken cancellationToken = default)
        => PaginationUtils.FetchPageAsync(dbContext.Products.AsNoTracking().OrderBy(x => x.Id), page, size, true, cancellationToken);

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<List<Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var list = ids.ToList();
        return dbContext.Products.Where(x => list.Contains(x.Id)).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        => await dbContext.Products.AddAsync(product, cancellationToken);

    public void Remove(Product product) => dbContext.Products.Remove(product);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
