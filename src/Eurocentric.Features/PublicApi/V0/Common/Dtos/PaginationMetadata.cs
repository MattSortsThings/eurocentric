using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.Common.Dtos;

public sealed record PaginationMetadata : IExampleProvider<PaginationMetadata>
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public bool Descending { get; init; }

    public int TotalPages { get; init; }

    public int TotalItems { get; init; }

    public static PaginationMetadata CreateExample() => new()
    {
        PageIndex = QueryParamDefaults.PageIndex,
        PageSize = QueryParamDefaults.PageSize,
        Descending = QueryParamDefaults.Descending,
        TotalItems = 40,
        TotalPages = 4
    };
}
