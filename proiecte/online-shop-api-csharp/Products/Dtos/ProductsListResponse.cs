namespace OnlineShopApi.Dtos;

public record ProductsListResponse(
    List<ProductsResponse> ProductsList,
    long TotalElements,
    int TotalPages,
    int PageNumber,
    int PageSize
);
