using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.Competitors;

public abstract record PointsAverageQuery
    : IOptionalBroadcastFiltering,
        IOptionalPaginationSettings,
        IOptionalVotingMethodFiltering
{
    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public ContestStageFilter? ContestStage { get; init; }

    public int? PageIndex { get; init; }

    public int? PageSize { get; init; }

    public bool? Descending { get; init; }

    public VotingMethodFilter? VotingMethod { get; init; }
}
