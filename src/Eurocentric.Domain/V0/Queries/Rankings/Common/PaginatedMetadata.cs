namespace Eurocentric.Domain.V0.Queries.Rankings.Common;

public abstract record PaginatedMetadata
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public bool Descending { get; init; }

    public int TotalItems { get; init; }

    public int TotalPages { get; init; }
}
