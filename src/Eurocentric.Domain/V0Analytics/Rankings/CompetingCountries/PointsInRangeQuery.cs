using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0Analytics.Rankings.Common;

namespace Eurocentric.Domain.V0Analytics.Rankings.CompetingCountries;

public sealed record PointsInRangeQuery : IContestStagesFilter,
    IPaginatedQuery,
    IPointsInRangeQuery,
    IVotingCountryFilter,
    IVotingMethodFilter,
    IYearRangeFilter
{
    public ContestStage[]? ContestStages { get; init; }

    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public bool Descending { get; init; }

    public int MinPoints { get; init; }

    public int MaxPoints { get; init; }

    public string? VotingCountryCode { get; init; }

    public VotingMethod? VotingMethod { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }
}
