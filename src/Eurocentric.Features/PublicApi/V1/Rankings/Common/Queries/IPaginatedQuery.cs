namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;

internal interface IPaginatedQuery
{
    int PageIndex { get; }

    int PageSize { get; }

    bool Descending { get; }
}
