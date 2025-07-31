using Eurocentric.Features.PublicApi.V0.Common.DataAccess;
using Eurocentric.Features.PublicApi.V0.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V0.Common.Mappings;

internal static class Outbound
{
    internal static PaginationMetadata ToPaginationMetadataWithTotalItems(this PaginatedQuery query, int totalItems) => new()
    {
        PageIndex = query.PageIndex,
        PageSize = query.PageSize,
        Descending = query.Descending,
        TotalItems = totalItems,
        TotalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)query.PageSize)
    };
}
