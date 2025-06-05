namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies the current status of a contest.
/// </summary>
public enum ContestStatus
{
    /// <summary>
    ///     The contest is initialized: it has been created, but it has no child broadcasts.
    /// </summary>
    Initialized = 0,

    /// <summary>
    ///     The contest is in progress: it has one or more child broadcasts, but it does not have three completed child
    ///     broadcasts.
    /// </summary>
    InProgress = 1,

    /// <summary>
    ///     The contest is completed: it has three child broadcasts, and they are all completed.
    /// </summary>
    Completed = 2
}
