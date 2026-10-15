using OnlineShopApi.Dtos;

namespace OnlineShopApi.Services;

public interface IOrderDetailQueryService
{
    Task<OrderDetailsListResponse> FindAllOrderDetailsAsync(int page, int size, CancellationToken cancellationToken = default);
    Task<OrderDetailsResponse> FindOrderDetailByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
