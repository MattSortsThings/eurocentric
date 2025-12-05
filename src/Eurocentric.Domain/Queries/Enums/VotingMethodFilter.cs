namespace Eurocentric.Domain.Queries.Enums;

/// <summary>
///     Specifies the voting method filter to be applied to the queried voting data.
/// </summary>
public enum VotingMethodFilter
{
    /// <summary>
    ///     Filters the queried voting data to points awarded by any voting method (i.e. no filter).
    /// </summary>
    Any,

    /// <summary>
    ///     Filters the queried voting data to points awarded by televotes only.
    /// </summary>
    Televote,

    /// <summary>
    ///     Filters the queried voting data to points awarded by juries only.
    /// </summary>
    Jury,
}
