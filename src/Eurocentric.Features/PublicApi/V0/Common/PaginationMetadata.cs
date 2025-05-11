namespace Eurocentric.Features.PublicApi.V0.Common;

public sealed record PaginationMetadata(int PageIndex, int PageSize, int TotalPages, int TotalItems, bool Descending)
{
    internal static PaginationMetadata Create(int totalItems = 0,
        int pageIndex = PaginationDefaults.PageIndex,
        int pageSize = PaginationDefaults.PageSize,
        bool descending = PaginationDefaults.Descending) => new(pageIndex,
        pageSize,
        (int)Math.Ceiling(totalItems / (double)pageSize),
        totalItems,
        descending);
}
