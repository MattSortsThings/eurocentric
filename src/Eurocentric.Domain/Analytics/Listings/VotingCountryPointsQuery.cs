using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     Parameters for a voting country points listings query.
/// </summary>
public abstract record VotingCountryPointsQuery : IRequiredBroadcastFiltering, IRequiredVotingCountryFiltering
{
    /// <inheritdoc />
    public required int ContestYear { get; init; }

    /// <inheritdoc />
    public required ContestStage ContestStage { get; init; }

    /// <inheritdoc />
    public required string VotingCountryCode { get; init; }
}
