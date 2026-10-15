using Microsoft.EntityFrameworkCore;

namespace OnlineShopApi.Common;

public record PageResult<T>(List<T> Items, long TotalElements, int TotalPages, int PageNumber, int PageSize);

public static class Pagination
{
    public static async Task<PageResult<T>> ToPageAsync<T>(
        this IQueryable<T> orderedQuery, int page, int size, bool fallbackToLastPage, CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(page, 0);
        var pageSize = Math.Max(size, 1);

        var totalElements = await orderedQuery.LongCountAsync(cancellationToken);
        var totalPages = (int)((totalElements + pageSize - 1) / pageSize);

        if (fallbackToLastPage && totalPages > 0 && pageNumber >= totalPages)
        {
            pageNumber = totalPages - 1;
        }

        var offset = (long)pageNumber * pageSize;
        var items = offset >= totalElements
            ? []
            : await orderedQuery.Skip((int)offset).Take(pageSize).ToListAsync(cancellationToken);

        return new PageResult<T>(items, totalElements, totalPages, pageNumber, pageSize);
    }
}
