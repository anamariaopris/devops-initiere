namespace OnlineShopApi.Dtos;

public record CustomerListResponse(
    List<CustomerResponse> Customers,
    long TotalElements,
    int TotalPages,
    int PageNumber,
    int PageSize
);
