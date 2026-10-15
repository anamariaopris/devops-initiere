using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Common;
using OnlineShopApi.Data;
using OnlineShopApi.Dtos;
using OnlineShopApi.Errors;
using OnlineShopApi.Models;

namespace OnlineShopApi.Services;

public class OrderDetailService(AppDbContext db)
{
    public async Task<OrderDetailsListResponse> GetPageAsync(int page, int size, CancellationToken cancellationToken)
    {
        var result = await db.OrderDetails.AsNoTracking().Include(d => d.Product).OrderBy(d => d.Id)
            .ToPageAsync(page, size, true, cancellationToken);
        return new OrderDetailsListResponse(
            result.Items.Select(d => d.ToResponse()).ToList(),
            result.TotalElements, result.TotalPages, result.PageNumber, result.PageSize);
    }

    public async Task<OrderDetailsResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        (await FindAsync(id, cancellationToken)).ToResponse();

    public async Task<OrderDetailsResponse> CreateAsync(OrderDetailsDto request, CancellationToken cancellationToken)
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken)
            ?? throw new NotFoundException("Product does not exist in the database.");
        var detail = request.ToEntity(product);
        db.OrderDetails.Add(detail);
        await db.SaveChangesAsync(cancellationToken);
        return detail.ToResponse();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        db.OrderDetails.Remove(await FindAsync(id, cancellationToken));
        await db.SaveChangesAsync(cancellationToken);
    }

    private async Task<OrderDetail> FindAsync(Guid id, CancellationToken cancellationToken) =>
        await db.OrderDetails.Include(d => d.Product).FirstOrDefaultAsync(d => d.Id == id, cancellationToken)
        ?? throw new NotFoundException("Order detail does not exist in the database.");
}
