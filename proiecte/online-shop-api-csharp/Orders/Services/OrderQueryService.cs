using OnlineShopApi.Common;
using OnlineShopApi.Dtos;
using OnlineShopApi.Exceptions;
using OnlineShopApi.Models;
using OnlineShopApi.Repositories;

namespace OnlineShopApi.Services;

public class OrderQueryService(IOrderRepository orderRepository, ICustomerRepository customerRepository) : IOrderQueryService
{
    public async Task<OrdersListResponse> FindAllOrdersAsync(int page, int size, CancellationToken cancellationToken = default)
        => ToListResponse(await orderRepository.GetPageAsync(page, size, cancellationToken));

    public async Task<OrdersResponse> FindOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(id, cancellationToken) ?? throw new OrderDoesntExistException();
        return order.ToResponse();
    }

    public async Task<OrdersListResponse> FindOrdersByCustomerIdAsync(Guid customerId, int page, int size, CancellationToken cancellationToken = default)
        => ToListResponse(await orderRepository.GetPageByCustomerIdAsync(customerId, page, size, cancellationToken));

    public async Task<OrdersListResponse> FindOrdersByEmailAsync(string email, int page, int size, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByUserEmailAsync(email, cancellationToken) ?? throw new CustomerDoesntExistException();
        return await FindOrdersByCustomerIdAsync(customer.Id, page, size, cancellationToken);
    }

    private static OrdersListResponse ToListResponse(PageResult<Order> result) =>
        new(result.Items.Select(o => o.ToResponse()).ToList(),
            result.TotalElements, result.TotalPages, result.PageNumber, result.PageSize);
}
