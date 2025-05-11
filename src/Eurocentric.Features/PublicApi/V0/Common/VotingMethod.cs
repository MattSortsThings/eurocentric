namespace Eurocentric.Features.PublicApi.V0.Common;

/// <summary>
///     Specifies the voting method used to determine the points awarded from a voting country to a competing country in a
///     broadcast.
/// </summary>
public enum VotingMethod
{
    /// <summary>
    ///     Points decided by any voting method.
    /// </summary>
    Any = 0,

    /// <summary>
    ///     Points decided by a national Jury.
    /// </summary>
    Jury = 1,

    /// <summary>
    ///     Points decided by a national Televote.
    /// </summary>
    Televote = 2
}
