namespace Eurocentric.Apis.Public.V1.Enums;

/// <summary>
///     Specifies the contest stage filter applied to voting data.
/// </summary>
public enum ContestStageFilter
{
    /// <summary>
    ///     Filters voting data to points awarded in any contest stage (i.e. no filter).
    /// </summary>
    Any,

    /// <summary>
    ///     Filters voting data to points awarded in the "Semi-Final 1" contest stage.
    /// </summary>
    SemiFinal1,

    /// <summary>
    ///     Filters voting data to points awarded in the "Semi-Final 2" contest stage.
    /// </summary>
    SemiFinal2,

    /// <summary>
    ///     Filters voting data to points awarded in the "Semi-Final 1" or "Semi-Final 2" contest stage.
    /// </summary>
    SemiFinals,

    /// <summary>
    ///     Filters voting data to points awarded in the "Grand Final" contest stage.
    /// </summary>
    GrandFinal,
}
