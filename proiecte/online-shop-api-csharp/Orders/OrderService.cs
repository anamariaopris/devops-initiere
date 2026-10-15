using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Common;
using OnlineShopApi.Data;
using OnlineShopApi.Dtos;
using OnlineShopApi.Errors;
using OnlineShopApi.Models;

namespace OnlineShopApi.Services;

public class OrderService(AppDbContext db)
{
    public Task<OrdersListResponse> GetPageAsync(int page, int size, CancellationToken cancellationToken) =>
        PageAsync(WithDetails(), page, size, true, cancellationToken);

    public Task<OrdersListResponse> GetByCustomerAsync(Guid customerId, int page, int size, CancellationToken cancellationToken) =>
        PageAsync(WithDetails().Where(o => o.CustomerId == customerId), page, size, false, cancellationToken);

    public async Task<OrdersListResponse> GetMineAsync(string email, int page, int size, CancellationToken cancellationToken)
    {
        var customer = await FindCustomerAsync(c => c.User.Email == email, cancellationToken);
        return await GetByCustomerAsync(customer.Id, page, size, cancellationToken);
    }

    public async Task<OrdersResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        (await FindAsync(id, cancellationToken)).ToResponse();

    public async Task<OrdersResponse> CreateAsync(Guid customerId, OrdersDto request, CancellationToken cancellationToken) =>
        await SaveAsync(await FindCustomerAsync(c => c.Id == customerId, cancellationToken), request, cancellationToken);

    public async Task<OrdersResponse> CreateMineAsync(string email, OrdersDto request, CancellationToken cancellationToken) =>
        await SaveAsync(await FindCustomerAsync(c => c.User.Email == email, cancellationToken), request, cancellationToken);

    public async Task<OrdersResponse> UpdateStatusAsync(Guid id, string status, CancellationToken cancellationToken)
    {
        var order = await FindAsync(id, cancellationToken);
        order.OrderStatus = status;
        await db.SaveChangesAsync(cancellationToken);
        return order.ToResponse();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        db.Orders.Remove(await FindAsync(id, cancellationToken));
        await db.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> IsOwnerAsync(Guid orderId, string email, CancellationToken cancellationToken) =>
        db.Orders.AnyAsync(o => o.Id == orderId && o.Customer != null && o.Customer.User.Email == email, cancellationToken);

    private IQueryable<Order> WithDetails() =>
        db.Orders.Include(o => o.OrderDetails).ThenInclude(d => d.Product);

    private static async Task<OrdersListResponse> PageAsync(
        IQueryable<Order> query, int page, int size, bool fallbackToLastPage, CancellationToken cancellationToken)
    {
        var result = await query.AsNoTracking().OrderBy(o => o.Id).ToPageAsync(page, size, fallbackToLastPage, cancellationToken);
        return new OrdersListResponse(
            result.Items.Select(o => o.ToResponse()).ToList(),
            result.TotalElements, result.TotalPages, result.PageNumber, result.PageSize);
    }

    private async Task<Order> FindAsync(Guid id, CancellationToken cancellationToken) =>
        await WithDetails().FirstOrDefaultAsync(o => o.Id == id, cancellationToken)
        ?? throw new NotFoundException("Order does not exist in the database.");

    private async Task<Customer> FindCustomerAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken) =>
        await db.Customers.FirstOrDefaultAsync(predicate, cancellationToken)
        ?? throw new NotFoundException("Customer does not exist in the database.");

    private async Task<OrdersResponse> SaveAsync(Customer customer, OrdersDto request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Customer = customer,
            Ammount = request.Ammount,
            ShippingAdress = request.ShippingAdress.Trim(),
            OrderAdress = request.OrderAddress.Trim(),
            OrderEmail = request.OrderEmail.Trim(),
            OrderDate = request.OrderDate!.Value,
            OrderStatus = request.OrderStatus.Trim()
        };

        var lines = request.OrderDetailsSet ?? [];
        var productIds = lines.Select(l => l.ProductId!.Value).Distinct().ToList();
        var products = await db.Products.Where(p => productIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id, cancellationToken);
        if (products.Count != productIds.Count)
        {
            throw new NotFoundException("Product does not exist in the database.");
        }

        foreach (var line in lines)
        {
            order.OrderDetails.Add(line.ToEntity(products[line.ProductId!.Value]));
        }

        db.Orders.Add(order);
        await db.SaveChangesAsync(cancellationToken);
        return order.ToResponse();
    }
}
