using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

public sealed record PaginationMetadata : IExampleProvider<PaginationMetadata>
{
    public required int PageIndex { get; init; }

    public required int PageSize { get; init; }

    public required bool Descending { get; init; }

    public required int TotalItems { get; init; }

    public required int TotalPages { get; init; }

    public static PaginationMetadata CreateExample() => new()
    {
        PageIndex = QueryParamDefaults.PageIndex,
        PageSize = QueryParamDefaults.PageSize,
        Descending = QueryParamDefaults.Descending,
        TotalItems = 40,
        TotalPages = 4
    };
}
