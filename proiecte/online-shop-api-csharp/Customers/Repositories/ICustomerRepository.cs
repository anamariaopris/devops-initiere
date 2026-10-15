using OnlineShopApi.Common;
using OnlineShopApi.Models;

namespace OnlineShopApi.Repositories;

public interface ICustomerRepository
{
    Task<PageResult<Customer>> GetPageAsync(int page, int size, CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Customer?> GetByUserEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
    void Remove(Customer customer);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
