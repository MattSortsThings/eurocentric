using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.Competitors;

/// <summary>
///     Metadata describing an executed competitor points share rankings query.
/// </summary>
public sealed record PointsShareMetadata : PaginatedMetadata
{
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
    ///     The optional competing country code filter value.
    /// </summary>
    public string? CompetingCountryCode { get; init; }

    /// <summary>
    ///     The optional voting method filter value.
    /// </summary>
    public VotingMethodFilter? VotingMethod { get; init; }
}
