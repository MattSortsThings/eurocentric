using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.Competitors;

/// <summary>
///     Parameters for a competitor points consensus rankings query.
/// </summary>
public abstract record PointsConsensusQuery
    : IOptionalBroadcastFiltering,
        IOptionalCompetingCountryFiltering,
        IOptionalPaginationOverrides
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
    public int? PageIndex { get; init; }

    /// <inheritdoc />
    public int? PageSize { get; init; }

    /// <inheritdoc />
    public bool? Descending { get; init; }
}
