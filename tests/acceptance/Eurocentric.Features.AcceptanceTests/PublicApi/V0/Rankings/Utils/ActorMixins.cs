using Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Rankings.Utils;

internal static class ActorMixins
{
    internal static void Then_the_response_pagination_data_should_match<T>(this ActorWithResponse<T> actor,
        bool descending = false,
        int totalItems = 0,
        int totalPages = 0,
        int pageIndex = 0,
        int pageSize = 0) where T : PaginatedResponseBase
    {
        Assert.NotNull(actor.ResponseObject);

        PaginationInfo expected = new()
        {
            TotalItems = totalItems,
            TotalPages = totalPages,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Descending = descending
        };

        Assert.Equal(expected, actor.ResponseObject.Pagination);
    }
}
