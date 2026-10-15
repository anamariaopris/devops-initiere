using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Common;
using OnlineShopApi.Data;
using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public class OrderDetailRepository(AppDbContext dbContext) : IOrderDetailRepository
{
    public Task<PageResult<OrderDetail>> GetPageAsync(int page, int size, CancellationToken cancellationToken = default)
        => PaginationUtils.FetchPageAsync(
            dbContext.OrderDetails.AsNoTracking().Include(x => x.Product).OrderBy(x => x.Id),
            page, size, true, cancellationToken);

    public Task<OrderDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.OrderDetails.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(OrderDetail orderDetail, CancellationToken cancellationToken = default)
        => await dbContext.OrderDetails.AddAsync(orderDetail, cancellationToken);

    public void Remove(OrderDetail orderDetail) => dbContext.OrderDetails.Remove(orderDetail);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
