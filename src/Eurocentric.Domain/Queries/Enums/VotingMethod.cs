namespace Eurocentric.Domain.Queries.Enums;

/// <summary>
///     Specifies the voting method filter applied to the queryable voting data.
/// </summary>
public enum VotingMethod
{
    /// <summary>
    ///     Filters the queryable voting data to points awarded by any method (i.e. no filter).
    /// </summary>
    Any,

    /// <summary>
    ///     Filters the queryable voting data to points awarded by televotes.
    /// </summary>
    Televote,

    /// <summary>
    ///     Filters the queryable voting data to points awarded by juries.
    /// </summary>
    Jury,
}
