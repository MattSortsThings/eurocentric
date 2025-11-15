using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     Metadata describing an executed voting country points listings query.
/// </summary>
public sealed record VotingCountryPointsMetadata
{
    /// <summary>
    ///     The required contest year filter value.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The required contest stage filter value.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     The required voting country code filter value.
    /// </summary>
    public string VotingCountryCode { get; init; } = string.Empty;
}
