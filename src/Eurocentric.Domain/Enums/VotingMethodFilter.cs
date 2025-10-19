namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies the voting method filter applied to voting data.
/// </summary>
public enum VotingMethodFilter
{
    /// <summary>
    ///     Filters voting data to points awarded by any method (i.e. no filter).
    /// </summary>
    Any,

    /// <summary>
    ///     Filters voting data to points awarded by juries.
    /// </summary>
    Jury,

    /// <summary>
    ///     Filters voting data to points awarded by televotes.
    /// </summary>
    Televote,
}
