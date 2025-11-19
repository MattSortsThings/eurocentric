using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.CompetingCountries;

/// <summary>
///     Metadata describing an executed competing country points average rankings query.
/// </summary>
public sealed record PointsAverageMetadata : PaginatedMetadata
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
    ///     The optional voting country code filter value.
    /// </summary>
    public string? VotingCountryCode { get; init; }

    /// <summary>
    ///     The optional voting method filter value.
    /// </summary>
    public VotingMethodFilter? VotingMethod { get; init; }
}
