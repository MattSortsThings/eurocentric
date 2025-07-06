namespace Eurocentric.Features.PublicApi.V0.Common.Contracts;

/// <summary>
///     Specifies a voting method filter applied to the queryable voting points awards.
/// </summary>
public enum VotingMethodFilter
{
    /// <summary>
    ///     Applies no voting method filter to the queryable points awards.
    /// </summary>
    Any = 0,

    /// <summary>
    ///     Restricts the queryable points awards to those decided by juries only.
    /// </summary>
    Jury = 1,

    /// <summary>
    ///     Restricts the queryable points awards to those decided by televotes only.
    /// </summary>
    Televote = 2
}
