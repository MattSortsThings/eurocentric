namespace Eurocentric.Domain.V0Analytics.Rankings.Common;

/// <summary>
///     Specifies the voting method used for one or more points awards.
/// </summary>
public enum VotingMethod
{
    /// <summary>
    ///     Points awarded by any method.
    /// </summary>
    Any = 0,

    /// <summary>
    ///     Points awarded by a jury.
    /// </summary>
    Jury = 1,

    /// <summary>
    ///     Points awarded by a televote.
    /// </summary>
    Televote = 2
}
