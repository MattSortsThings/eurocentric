using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Queries.Listings;

public abstract record BroadcastResultQuery
{
    /// <summary>
    ///     Gets the year in which the contest is held.
    /// </summary>
    public required int ContestYear { get; init; }

    /// <summary>
    ///     Gets the broadcast's stage in its parent contest.
    /// </summary>
    public required ContestStage ContestStage { get; init; }
}
