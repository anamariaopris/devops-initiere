using OnlineShopApi.Dtos;

namespace OnlineShopApi.Services;

public interface ICustomerQueryService
{
    Task<CustomerListResponse> FindAllCustomersAsync(int page, int size, CancellationToken cancellationToken = default);
    Task<CustomerResponse> FindCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CustomerResponse> FindCustomerByEmailAsync(string email, CancellationToken cancellationToken = default);
}
