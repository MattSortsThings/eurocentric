namespace Eurocentric.Domain.V0.Rankings.Common;

public abstract record PaginationMetadata
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public bool Descending { get; init; }

    public int TotalPages { get; init; }

    public int TotalItems { get; init; }
}
