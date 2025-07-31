namespace Eurocentric.Features.PublicApi.V0.Common.DataAccess;

internal abstract record PaginatedQuery
{
    public required int PageIndex { get; init; }

    public required int PageSize { get; init; }

    public required bool Descending { get; init; }
}
