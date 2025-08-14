namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;

internal interface IPaginatedQuery
{
    public int PageIndex { get; }

    public int PageSize { get; }

    public bool Descending { get; }
}
