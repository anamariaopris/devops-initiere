using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Common;
using OnlineShopApi.Data;
using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public class CustomerRepository(AppDbContext dbContext) : ICustomerRepository
{
    private IQueryable<Customer> WithGraph() => dbContext.Customers
        .Include(x => x.User)
        .Include(x => x.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(d => d.Product);

    public Task<PageResult<Customer>> GetPageAsync(int page, int size, CancellationToken cancellationToken = default)
        => PaginationUtils.FetchPageAsync(WithGraph().AsNoTracking().OrderBy(x => x.Id), page, size, true, cancellationToken);

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => WithGraph().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Customer?> GetByUserEmailAsync(string email, CancellationToken cancellationToken = default)
        => WithGraph().FirstOrDefaultAsync(x => x.User.Email == email, cancellationToken);

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
        => await dbContext.Customers.AddAsync(customer, cancellationToken);

    public void Remove(Customer customer)
    {
        dbContext.Customers.Remove(customer);
        dbContext.Users.Remove(customer.User);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
