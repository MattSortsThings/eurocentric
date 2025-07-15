namespace Eurocentric.Features.AdminApi.V1.Common.Contracts;

public sealed record BroadcastMemo
{
    /// <summary>
    ///     The broadcast's unique ID.
    /// </summary>
    public required Guid BroadcastId { get; init; }

    /// <summary>
    ///     The broadcast's stage in its parent contest.
    /// </summary>
    public required ContestStage ContestStage { get; init; }

    /// <summary>
    ///     The broadcast's status.
    /// </summary>
    public required BroadcastStatus BroadcastStatus { get; init; }
}
