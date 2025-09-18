namespace Eurocentric.Domain.V0Analytics.Rankings.Common;

public abstract record PaginatedMetadata
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public bool Descending { get; init; }

    public int TotalRankings { get; init; }

    public int TotalPages { get; init; }
}
