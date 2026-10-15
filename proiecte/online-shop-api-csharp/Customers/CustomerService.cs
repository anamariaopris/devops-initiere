using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Common;
using OnlineShopApi.Data;
using OnlineShopApi.Dtos;
using OnlineShopApi.Errors;
using OnlineShopApi.Models;

namespace OnlineShopApi.Services;

public class CustomerService(AppDbContext db)
{
    public async Task<CustomerListResponse> GetPageAsync(int page, int size, CancellationToken cancellationToken)
    {
        var result = await WithGraph().AsNoTracking().OrderBy(c => c.Id).ToPageAsync(page, size, true, cancellationToken);
        return new CustomerListResponse(
            result.Items.Select(c => c.ToResponse()).ToList(),
            result.TotalElements, result.TotalPages, result.PageNumber, result.PageSize);
    }

    public async Task<CustomerResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        (await FindAsync(c => c.Id == id, cancellationToken)).ToResponse();

    public async Task<CustomerResponse> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        (await FindAsync(c => c.User.Email == email, cancellationToken)).ToResponse();

    public async Task<CustomerResponse> UpdateAsync(Guid id, CustomerDto request, CancellationToken cancellationToken) =>
        await ApplyAsync(await FindAsync(c => c.Id == id, cancellationToken), request, cancellationToken);

    public async Task<CustomerResponse> UpdateByEmailAsync(string email, CustomerDto request, CancellationToken cancellationToken) =>
        await ApplyAsync(await FindAsync(c => c.User.Email == email, cancellationToken), request, cancellationToken);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await FindAsync(c => c.Id == id, cancellationToken);
        db.Customers.Remove(customer);
        db.Users.Remove(customer.User);
        await db.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> IsOwnerAsync(Guid customerId, string email, CancellationToken cancellationToken) =>
        db.Customers.AnyAsync(c => c.Id == customerId && c.User.Email == email, cancellationToken);

    private IQueryable<Customer> WithGraph() => db.Customers
        .Include(c => c.User)
        .Include(c => c.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(d => d.Product);

    private async Task<Customer> FindAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken) =>
        await WithGraph().FirstOrDefaultAsync(predicate, cancellationToken)
        ?? throw new NotFoundException("Customer does not exist in the database.");

    private async Task<CustomerResponse> ApplyAsync(Customer customer, CustomerDto request, CancellationToken cancellationToken)
    {
        customer.FullName = request.FullName;
        customer.BillingAddress = request.BillingAddress;
        customer.DefaultShippingAddress = request.DefaultShippingAddress;
        customer.Country = request.Country;
        customer.Phone = request.Phone;
        await db.SaveChangesAsync(cancellationToken);
        return customer.ToResponse();
    }
}
