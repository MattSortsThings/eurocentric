using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.VotingCountries;

/// <summary>
///     Metadata describing an executed voting country points in range rankings query.
/// </summary>
public sealed record PointsInRangeMetadata : PaginatedMetadata
{
    /// <summary>
    ///     The required competing country code filter value.
    /// </summary>
    public string CompetingCountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The required inclusive minimum points value.
    /// </summary>
    public int MinPoints { get; init; }

    /// <summary>
    ///     The required inclusive maximum points value.
    /// </summary>
    public int MaxPoints { get; init; }

    /// <summary>
    ///     The optional inclusive minimum contest year filter value.
    /// </summary>
    public int? MinYear { get; init; }

    /// <summary>
    ///     The optional inclusive maximum contest year filter value.
    /// </summary>
    public int? MaxYear { get; init; }

    /// <summary>
    ///     The optional contest stage filter value.
    /// </summary>
    public ContestStageFilter? ContestStage { get; init; }

    /// <summary>
    ///     The optional voting method filter value.
    /// </summary>
    public VotingMethodFilter? VotingMethod { get; init; }
}
