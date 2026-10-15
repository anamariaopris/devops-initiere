using OnlineShopApi.Dtos;

namespace OnlineShopApi.Services;

public interface IOrderDetailCommandService
{
    Task<OrderDetailsResponse> CreateOrderDetailsAsync(OrderDetailsDto dto, CancellationToken cancellationToken = default);
    Task DeleteOrderDetailAsync(Guid id, CancellationToken cancellationToken = default);
}
