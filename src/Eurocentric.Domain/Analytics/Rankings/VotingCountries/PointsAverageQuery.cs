using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.VotingCountries;

public abstract record PointsAverageQuery
    : IRequiredCompetingCountryFiltering,
        IOptionalBroadcastFiltering,
        IOptionalPaginationSettings,
        IOptionalVotingMethodFiltering
{
    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public ContestStageFilter? ContestStage { get; init; }

    public bool? Descending { get; init; }

    public int? PageIndex { get; init; }

    public int? PageSize { get; init; }

    public VotingMethodFilter? VotingMethod { get; init; }

    public required string CompetingCountryCode { get; init; }
}
