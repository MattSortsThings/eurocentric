namespace Eurocentric.Features.AdminApi.V1.Common.Contracts;

/// <summary>
///     Specifies a single broadcast's status.
/// </summary>
public enum BroadcastStatus
{
    /// <summary>
    ///     The broadcast has been initialized and none of its juries and/or televotes has awarded its points.
    /// </summary>
    Initialized = 0,

    /// <summary>
    ///     The broadcast has been initialized and at least one (but not all) of its juries and/or televotes has awarded its
    ///     points.
    /// </summary>
    InProgress = 1,

    /// <summary>
    ///     Every jury and/or televote in the broadcast has awarded its points.
    /// </summary>
    Completed = 2
}
