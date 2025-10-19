using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Queries.Listings;

public sealed record BroadcastResultMetadata
{
    /// <summary>
    ///     Gets the year in which the contest is held.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     Gets the broadcast's stage in its parent contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }
}
