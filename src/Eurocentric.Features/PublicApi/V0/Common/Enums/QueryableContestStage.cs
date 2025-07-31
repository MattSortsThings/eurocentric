namespace Eurocentric.Features.PublicApi.V0.Common.Enums;

/// <summary>
///     Filters the queryable data by the contest stage in which the points were awarded.
/// </summary>
public enum QueryableContestStage
{
    /// <summary>
    ///     Points awarded in any contest stage (i.e. no filter applied).
    /// </summary>
    Any = 0,

    /// <summary>
    ///     Points awarded in First Semi-Final broadcasts only.
    /// </summary>
    SemiFinal1 = 1,

    /// <summary>
    ///     Points awarded in Second Semi-Final broadcasts only.
    /// </summary>
    SemiFinal2 = 2,

    /// <summary>
    ///     Points awarded in First or Second Semi-Final broadcasts only.
    /// </summary>
    SemiFinals = 3,

    /// <summary>
    ///     Points awarded in Grand Final broadcasts only.
    /// </summary>
    GrandFinal = 4
}
