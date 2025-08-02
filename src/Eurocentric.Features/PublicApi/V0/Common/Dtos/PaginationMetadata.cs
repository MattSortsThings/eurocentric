namespace Eurocentric.Features.PublicApi.V0.Common.Dtos;

public sealed record PaginationMetadata
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public bool Descending { get; init; }

    public int TotalPages { get; init; }

    public int TotalItems { get; init; }
}
