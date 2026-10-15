using OnlineShopApi.Common;
using OnlineShopApi.Dtos;
using OnlineShopApi.Exceptions;
using OnlineShopApi.Repositories;

namespace OnlineShopApi.Services;

public class CustomerQueryService(ICustomerRepository customerRepository) : ICustomerQueryService
{
    public async Task<CustomerListResponse> FindAllCustomersAsync(int page, int size, CancellationToken cancellationToken = default)
    {
        var result = await customerRepository.GetPageAsync(page, size, cancellationToken);
        return new CustomerListResponse(
            result.Items.Select(c => c.ToResponse()).ToList(),
            result.TotalElements, result.TotalPages, result.PageNumber, result.PageSize);
    }

    public async Task<CustomerResponse> FindCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByIdAsync(id, cancellationToken) ?? throw new CustomerDoesntExistException();
        return customer.ToResponse();
    }

    public async Task<CustomerResponse> FindCustomerByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByUserEmailAsync(email, cancellationToken) ?? throw new CustomerDoesntExistException();
        return customer.ToResponse();
    }
}
