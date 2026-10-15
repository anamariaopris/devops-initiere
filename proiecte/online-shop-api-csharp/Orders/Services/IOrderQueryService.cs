using OnlineShopApi.Dtos;

namespace OnlineShopApi.Services;

public interface IOrderQueryService
{
    Task<OrdersListResponse> FindAllOrdersAsync(int page, int size, CancellationToken cancellationToken = default);
    Task<OrdersResponse> FindOrderByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrdersListResponse> FindOrdersByCustomerIdAsync(Guid customerId, int page, int size, CancellationToken cancellationToken = default);
    Task<OrdersListResponse> FindOrdersByEmailAsync(string email, int page, int size, CancellationToken cancellationToken = default);
}
