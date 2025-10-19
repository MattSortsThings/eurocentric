using Eurocentric.Apis.Public.V0.Enums;

namespace Eurocentric.Apis.Public.V0.Dtos.Listings;

public sealed record BroadcastResultMetadata
{
    /// <summary>
    ///     The year in which the contest is held.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The broadcast's stage in its parent contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }
}
