using OnlineShopApi.Common;
using OnlineShopApi.Dtos;
using OnlineShopApi.Exceptions;
using OnlineShopApi.Models;
using OnlineShopApi.Repositories;

namespace OnlineShopApi.Services;

public class OrderCommandService(
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    IProductRepository productRepository) : IOrderCommandService
{
    public async Task<OrdersResponse> CreateOrderAsync(Guid customerId, OrdersDto dto, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByIdAsync(customerId, cancellationToken) ?? throw new CustomerDoesntExistException();
        return await SaveOrderForAsync(customer, dto, cancellationToken);
    }

    public async Task<OrdersResponse> CreateOrderForCurrentUserAsync(string email, OrdersDto dto, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByUserEmailAsync(email, cancellationToken) ?? throw new CustomerDoesntExistException();
        return await SaveOrderForAsync(customer, dto, cancellationToken);
    }

    public async Task<OrdersResponse> UpdateOrderStatusAsync(Guid id, string? status, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(id, cancellationToken) ?? throw new OrderDoesntExistException();
        order.OrderStatus = status;
        await orderRepository.SaveChangesAsync(cancellationToken);
        return order.ToResponse();
    }

    public async Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(id, cancellationToken) ?? throw new OrderDoesntExistException();
        orderRepository.Remove(order);
        await orderRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<OrdersResponse> SaveOrderForAsync(Customer customer, OrdersDto dto, CancellationToken cancellationToken)
    {
        var order = dto.ToEntity();
        order.Customer = customer;

        if (dto.OrderDetailsSet is { Count: > 0 })
        {
            await AttachOrderDetailsAsync(order, dto.OrderDetailsSet, cancellationToken);
        }

        await orderRepository.AddAsync(order, cancellationToken);
        await orderRepository.SaveChangesAsync(cancellationToken);
        return order.ToResponse();
    }

    private async Task AttachOrderDetailsAsync(Order order, List<OrderDetailsDto> detailDtos, CancellationToken cancellationToken)
    {
        if (detailDtos.Any(d => d.ProductId is null))
        {
            throw new ProductDoesntExistException();
        }

        var productIds = detailDtos.Select(d => d.ProductId!.Value).Distinct().ToList();
        var productsById = (await productRepository.GetByIdsAsync(productIds, cancellationToken)).ToDictionary(p => p.Id);

        if (productsById.Count != productIds.Count)
        {
            throw new ProductDoesntExistException();
        }

        foreach (var detailDto in detailDtos)
        {
            var detail = detailDto.ToEntity();
            detail.Product = productsById[detailDto.ProductId!.Value];
            detail.Order = order;
            order.OrderDetails.Add(detail);
        }
    }
}
