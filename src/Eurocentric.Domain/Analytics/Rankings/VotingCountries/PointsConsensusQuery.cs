using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.VotingCountries;

/// <summary>
///     Parameters for a voting country points consensus rankings query.
/// </summary>
public abstract record PointsConsensusQuery
    : IOptionalBroadcastFiltering,
        IOptionalPaginationOverrides,
        IOptionalCompetingCountryFiltering
{
    /// <inheritdoc />
    public int? MinYear { get; init; }

    /// <inheritdoc />
    public int? MaxYear { get; init; }

    /// <inheritdoc />
    public ContestStageFilter? ContestStage { get; init; }

    /// <inheritdoc />
    public string? CompetingCountryCode { get; init; }

    /// <inheritdoc />
    public bool? Descending { get; init; }

    /// <inheritdoc />
    public int? PageIndex { get; init; }

    /// <inheritdoc />
    public int? PageSize { get; init; }
}
