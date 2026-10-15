namespace OnlineShopApi.Dtos;

public record OrderDetailsListResponse(
    List<OrderDetailsResponse> OrderDetailsList,
    long TotalElements,
    int TotalPages,
    int PageNumber,
    int PageSize
);
