using OnlineShopApi.Dtos;

namespace OnlineShopApi.Services;

public interface IOrderCommandService
{
    Task<OrdersResponse> CreateOrderAsync(Guid customerId, OrdersDto dto, CancellationToken cancellationToken = default);
    Task<OrdersResponse> CreateOrderForCurrentUserAsync(string email, OrdersDto dto, CancellationToken cancellationToken = default);
    Task<OrdersResponse> UpdateOrderStatusAsync(Guid id, string? status, CancellationToken cancellationToken = default);
    Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default);
}
