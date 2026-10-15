using OnlineShopApi.Dtos;

namespace OnlineShopApi.Services;

public interface ICustomerCommandService
{
    Task<CustomerResponse> UpdateCustomerAsync(Guid id, CustomerDto dto, CancellationToken cancellationToken = default);
    Task<CustomerResponse> UpdateCustomerByEmailAsync(string email, CustomerDto dto, CancellationToken cancellationToken = default);
    Task DeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default);
}
