using OnlineShopApi.Common;
using OnlineShopApi.Dtos;
using OnlineShopApi.Exceptions;
using OnlineShopApi.Repositories;

namespace OnlineShopApi.Services;

public class OrderDetailCommandService(
    IOrderDetailRepository orderDetailRepository,
    IProductRepository productRepository) : IOrderDetailCommandService
{
    public async Task<OrderDetailsResponse> CreateOrderDetailsAsync(OrderDetailsDto dto, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(dto.ProductId!.Value, cancellationToken) ?? throw new ProductDoesntExistException();
        var detail = dto.ToEntity();
        detail.Product = product;
        await orderDetailRepository.AddAsync(detail, cancellationToken);
        await orderDetailRepository.SaveChangesAsync(cancellationToken);
        return detail.ToResponse();
    }

    public async Task DeleteOrderDetailAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var detail = await orderDetailRepository.GetByIdAsync(id, cancellationToken) ?? throw new OrderDetailsDoesntExistException();
        orderDetailRepository.Remove(detail);
        await orderDetailRepository.SaveChangesAsync(cancellationToken);
    }
}
