namespace Eurocentric.Domain.Enums;

/// <summary>
///     Filters the voting data by voting method.
/// </summary>
public enum VotingMethodFilter
{
    /// <summary>
    ///     Any voting method (i.e. no filter applied).
    /// </summary>
    Any = 0,

    /// <summary>
    ///     Jury points only.
    /// </summary>
    Jury = 1,

    /// <summary>
    ///     Televote points only.
    /// </summary>
    Televote = 2
}
