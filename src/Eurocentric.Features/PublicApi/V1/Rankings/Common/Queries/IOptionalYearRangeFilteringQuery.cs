namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;

internal interface IOptionalYearRangeFilteringQuery
{
    int? MinYear { get; }

    int? MaxYear { get; }
}
