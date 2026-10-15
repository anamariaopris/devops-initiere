using OnlineShopApi.Common;
using OnlineShopApi.Dtos;
using OnlineShopApi.Exceptions;
using OnlineShopApi.Repositories;

namespace OnlineShopApi.Services;

public class OrderDetailQueryService(IOrderDetailRepository orderDetailRepository) : IOrderDetailQueryService
{
    public async Task<OrderDetailsListResponse> FindAllOrderDetailsAsync(int page, int size, CancellationToken cancellationToken = default)
    {
        var result = await orderDetailRepository.GetPageAsync(page, size, cancellationToken);
        return new OrderDetailsListResponse(
            result.Items.Select(d => d.ToResponse()).ToList(),
            result.TotalElements, result.TotalPages, result.PageNumber, result.PageSize);
    }

    public async Task<OrderDetailsResponse> FindOrderDetailByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var detail = await orderDetailRepository.GetByIdAsync(id, cancellationToken) ?? throw new OrderDetailsDoesntExistException();
        return detail.ToResponse();
    }
}
