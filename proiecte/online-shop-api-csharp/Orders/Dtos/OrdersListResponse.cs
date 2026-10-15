namespace OnlineShopApi.Dtos;

public record OrdersListResponse(
    List<OrdersResponse> OrdersList,
    long TotalElements,
    int TotalPages,
    int PageNumber,
    int PageSize
);
