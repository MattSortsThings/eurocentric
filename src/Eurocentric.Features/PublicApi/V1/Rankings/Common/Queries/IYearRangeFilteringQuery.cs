namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;

internal interface IYearRangeFilteringQuery
{
    public int? MinYear { get; }

    public int? MaxYear { get; }
}
