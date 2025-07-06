namespace Eurocentric.Features.PublicApi.V0.Common.Contracts;

/// <summary>
///     Specifies a contest stage filter applied to the queryable voting points awards.
/// </summary>
public enum ContestStageFilter
{
    /// <summary>
    ///     Applies no contest stage filter to the queryable points awards.
    /// </summary>
    Any = 0,

    /// <summary>
    ///     Restricts the queryable points awards to those given in "First Semi-Final" broadcasts only.
    /// </summary>
    SemiFinal1 = 1,

    /// <summary>
    ///     Restricts the queryable points awards to those given in "Second Semi-Final" broadcasts only.
    /// </summary>
    SemiFinal2 = 2,

    /// <summary>
    ///     Restricts the queryable points awards to those given in "First Semi-Final" or "Second Semi-Final" broadcasts only.
    /// </summary>
    SemiFinals = 3,

    /// <summary>
    ///     Restricts the queryable points awards to those given in "Grand Final" broadcasts only.
    /// </summary>
    GrandFinal = 4
}
