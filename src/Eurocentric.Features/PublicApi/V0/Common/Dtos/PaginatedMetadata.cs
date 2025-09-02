namespace Eurocentric.Features.PublicApi.V0.Common.Dtos;

public abstract record PaginatedMetadata
{
    public required int PageIndex { get; init; }

    public required int PageSize { get; init; }

    public required bool Descending { get; init; }

    public required int TotalPages { get; init; }

    public required int TotalItems { get; init; }
}
