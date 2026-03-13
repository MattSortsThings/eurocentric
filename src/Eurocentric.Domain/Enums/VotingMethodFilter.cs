namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies the voting method filter to be applied to the queryable voting data.
/// </summary>
public enum VotingMethodFilter
{
    /// <summary>
    ///     Filters the queryable voting data to points awarded using any voting method; this is equivalent to no voting method
    ///     filter.
    /// </summary>
    Any,

    /// <summary>
    ///     Filters the queryable voting data to points awarded by juries only.
    /// </summary>
    Jury,

    /// <summary>
    ///     Filters the queryable voting data to points awarded by televotes only.
    /// </summary>
    Televote,
}
