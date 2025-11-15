using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     Metadata describing an executed broadcast result listings query.
/// </summary>
public sealed record BroadcastResultMetadata
{
    /// <summary>
    ///     The required contest year filter value.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The required contest stage filter value.
    /// </summary>
    public ContestStage ContestStage { get; init; }
}
