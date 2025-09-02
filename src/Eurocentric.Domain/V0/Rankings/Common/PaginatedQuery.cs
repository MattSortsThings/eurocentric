namespace Eurocentric.Domain.V0.Rankings.Common;

public abstract record PaginatedQuery
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public bool Descending { get; init; }
}
