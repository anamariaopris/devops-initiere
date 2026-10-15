using OnlineShopApi.Common;
using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public interface IOrderRepository
{
    Task<PageResult<Order>> GetPageAsync(int page, int size, CancellationToken cancellationToken = default);
    Task<PageResult<Order>> GetPageByCustomerIdAsync(Guid customerId, int page, int size, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    void Remove(Order order);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
