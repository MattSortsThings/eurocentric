namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;

internal interface IPointsRangeFilteringQuery
{
    int MinPoints { get; }

    int MaxPoints { get; }
}
