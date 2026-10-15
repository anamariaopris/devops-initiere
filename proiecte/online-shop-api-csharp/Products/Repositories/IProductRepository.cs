using OnlineShopApi.Common;
using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public interface IProductRepository
{
    Task<PageResult<Product>> GetPageAsync(int page, int size, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    void Remove(Product product);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
