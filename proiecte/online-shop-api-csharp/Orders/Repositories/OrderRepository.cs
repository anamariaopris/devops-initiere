using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Common;
using OnlineShopApi.Data;
using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public class OrderRepository(AppDbContext dbContext) : IOrderRepository
{
    private IQueryable<Order> WithDetails() => dbContext.Orders
        .Include(x => x.OrderDetails).ThenInclude(d => d.Product);

    public Task<PageResult<Order>> GetPageAsync(int page, int size, CancellationToken cancellationToken = default)
        => PaginationUtils.FetchPageAsync(WithDetails().AsNoTracking().OrderBy(x => x.Id), page, size, true, cancellationToken);

    public Task<PageResult<Order>> GetPageByCustomerIdAsync(Guid customerId, int page, int size, CancellationToken cancellationToken = default)
        => PaginationUtils.FetchPageAsync(
            WithDetails().AsNoTracking().Where(x => x.CustomerId == customerId).OrderBy(x => x.Id),
            page, size, false, cancellationToken);

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => WithDetails().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        => await dbContext.Orders.AddAsync(order, cancellationToken);

    public void Remove(Order order) => dbContext.Orders.Remove(order);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
