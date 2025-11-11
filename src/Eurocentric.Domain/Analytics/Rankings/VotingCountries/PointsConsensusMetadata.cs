using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0.Queries.Rankings.Common;

namespace Eurocentric.Domain.Analytics.Rankings.VotingCountries;

/// <summary>
///     Metadata describing an executed voting country points consensus rankings query.
/// </summary>
public sealed record PointsConsensusMetadata : PaginatedMetadata
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
}
