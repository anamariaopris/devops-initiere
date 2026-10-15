using OnlineShopApi.Common;
using OnlineShopApi.Dtos;
using OnlineShopApi.Exceptions;
using OnlineShopApi.Models;
using OnlineShopApi.Repositories;

namespace OnlineShopApi.Services;

public class CustomerCommandService(ICustomerRepository customerRepository) : ICustomerCommandService
{
    public async Task<CustomerResponse> UpdateCustomerAsync(Guid id, CustomerDto dto, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByIdAsync(id, cancellationToken) ?? throw new CustomerDoesntExistException();
        return await ApplyAndSaveAsync(customer, dto, cancellationToken);
    }

    public async Task<CustomerResponse> UpdateCustomerByEmailAsync(string email, CustomerDto dto, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByUserEmailAsync(email, cancellationToken) ?? throw new CustomerDoesntExistException();
        return await ApplyAndSaveAsync(customer, dto, cancellationToken);
    }

    public async Task DeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByIdAsync(id, cancellationToken) ?? throw new CustomerDoesntExistException();
        customerRepository.Remove(customer);
        await customerRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<CustomerResponse> ApplyAndSaveAsync(Customer customer, CustomerDto dto, CancellationToken cancellationToken)
    {
        customer.FullName = dto.FullName;
        customer.Phone = dto.Phone;
        customer.Country = dto.Country;
        customer.BillingAddress = dto.BillingAddress;
        customer.DefaultShippingAddress = dto.DefaultShippingAddress;
        await customerRepository.SaveChangesAsync(cancellationToken);
        return customer.ToResponse();
    }
}
