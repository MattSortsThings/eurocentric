using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0.Queries.Rankings.Common;

namespace Eurocentric.Domain.V0.Queries.Rankings.CompetingCountries;

public abstract record PointsAverageQuery
    : IOptionalBroadcastFiltering,
        IOptionalPaginationSettings,
        IOptionalVotingCountryFiltering,
        IOptionalVotingMethodFiltering
{
    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public ContestStageFilter? ContestStage { get; init; }

    public bool? Descending { get; init; }

    public int? PageIndex { get; init; }

    public int? PageSize { get; init; }

    public string? VotingCountryCode { get; init; }

    public VotingMethodFilter? VotingMethod { get; init; }
}
