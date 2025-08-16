namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;

internal interface IYearRangeFilteringQuery
{
    int? MinYear { get; }

    int? MaxYear { get; }
}
