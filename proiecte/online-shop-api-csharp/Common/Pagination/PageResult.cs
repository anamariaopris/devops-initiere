namespace OnlineShopApi.Common;

public record PageResult<T>(List<T> Items, long TotalElements, int TotalPages, int PageNumber, int PageSize);
