namespace Eurocentric.Features.PublicApi.V0.Common.Contracts;

public abstract record PaginatedResponseBase
{
    public required PaginationInfo Pagination { get; init; }
}
