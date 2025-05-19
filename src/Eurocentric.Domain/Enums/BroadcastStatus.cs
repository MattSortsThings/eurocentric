namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies the current status of a broadcast.
/// </summary>
public enum BroadcastStatus
{
    /// <summary>
    ///     The broadcast is initialized: it has been created, but none of its juries and/or televotes has awarded its points.
    /// </summary>
    Initialized = 0,

    /// <summary>
    ///     The broadcast is in progress: some but not all of its juries and/or televotes have awarded their points.
    /// </summary>
    InProgress = 1,

    /// <summary>
    ///     The broadcast is completed: all of its juries and/or televotes have awarded their points.
    /// </summary>
    Completed = 2
}
