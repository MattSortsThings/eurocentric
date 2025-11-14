using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.VotingCountries;

/// <summary>
///     Parameters for a voting country points in range query.
/// </summary>
public abstract record PointsInRangeQuery
    : IRequiredCompetingCountryFiltering,
        IRequiredPointsValueRange,
        IOptionalBroadcastFiltering,
        IOptionalPaginationOverrides,
        IOptionalVotingMethodFiltering
{
    /// <inheritdoc />
    public int? MinYear { get; init; }

    /// <inheritdoc />
    public int? MaxYear { get; init; }

    /// <inheritdoc />
    public ContestStageFilter? ContestStage { get; init; }

    /// <inheritdoc />
    public bool? Descending { get; init; }

    /// <inheritdoc />
    public int? PageIndex { get; init; }

    /// <inheritdoc />
    public int? PageSize { get; init; }

    public VotingMethodFilter? VotingMethod { get; init; }

    /// <inheritdoc />
    public required string CompetingCountryCode { get; init; }

    /// <inheritdoc />
    public int MinPoints { get; init; }

    /// <inheritdoc />
    public int MaxPoints { get; init; }
}
