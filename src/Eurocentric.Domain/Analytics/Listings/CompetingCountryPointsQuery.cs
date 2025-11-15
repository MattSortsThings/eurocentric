using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     Parameters for a competing country points listings query.
/// </summary>
public abstract record CompetingCountryPointsQuery : IRequiredBroadcastFiltering, IRequiredCompetingCountryFiltering
{
    /// <inheritdoc />
    public required int ContestYear { get; init; }

    /// <inheritdoc />
    public required ContestStage ContestStage { get; init; }

    /// <inheritdoc />
    public required string CompetingCountryCode { get; init; }
}
