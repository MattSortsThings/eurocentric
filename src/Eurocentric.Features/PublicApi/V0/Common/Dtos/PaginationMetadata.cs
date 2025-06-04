using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.Common.Dtos;

public sealed record PaginationMetadata : IExampleProvider<PaginationMetadata>
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public int TotalItems { get; init; }

    public int TotalPages { get; init; }

    public bool Descending { get; init; }

    public static PaginationMetadata CreateExample() => new()
    {
        PageIndex = 0, PageSize = 10, TotalItems = 40, TotalPages = 4, Descending = false
    };
}
