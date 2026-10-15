using OnlineShopApi.Common;
using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public interface IOrderDetailRepository
{
    Task<PageResult<OrderDetail>> GetPageAsync(int page, int size, CancellationToken cancellationToken = default);
    Task<OrderDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(OrderDetail orderDetail, CancellationToken cancellationToken = default);
    void Remove(OrderDetail orderDetail);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
