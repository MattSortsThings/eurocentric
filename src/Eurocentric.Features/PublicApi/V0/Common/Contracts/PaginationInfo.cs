using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.Common.Contracts;

public sealed record PaginationInfo : IExampleProvider<PaginationInfo>
{
    public required int PageIndex { get; init; }

    public required int PageSize { get; init; }

    public required int TotalItems { get; init; }

    public required int TotalPages { get; init; }

    public required bool Descending { get; init; }

    public static PaginationInfo CreateExample() => new()
    {
        PageIndex = QueryParameterDefaults.PageIndex,
        PageSize = QueryParameterDefaults.PageSize,
        Descending = QueryParameterDefaults.Descending,
        TotalItems = 50,
        TotalPages = 5
    };
}
