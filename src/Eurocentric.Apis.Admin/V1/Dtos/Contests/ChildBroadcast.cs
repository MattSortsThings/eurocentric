using Eurocentric.Apis.Admin.V1.Enums;

namespace Eurocentric.Apis.Admin.V1.Dtos.Contests;

/// <summary>
///     Summarizes a broadcast from the perspective of its parent contest.
/// </summary>
public sealed record ChildBroadcast
{
    /// <summary>
    ///     The ID of the child broadcast.
    /// </summary>
    public Guid ChildBroadcastId { get; init; }

    /// <summary>
    ///     The child broadcast's stage in the contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     A boolean value indicating whether the child broadcast is completed.
    /// </summary>
    public bool Completed { get; init; }
}
