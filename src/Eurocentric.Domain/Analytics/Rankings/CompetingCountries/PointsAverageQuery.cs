using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.CompetingCountries;

/// <summary>
///     Parameters for a competing country points average rankings query.
/// </summary>
public abstract record PointsAverageQuery
    : IOptionalBroadcastFiltering,
        IOptionalPaginationOverrides,
        IOptionalVotingCountryFiltering,
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

    /// <inheritdoc />
    public string? VotingCountryCode { get; init; }

    /// <inheritdoc />
    public VotingMethodFilter? VotingMethod { get; init; }
}
