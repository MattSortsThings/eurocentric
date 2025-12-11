namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies the contest stages filter applied to the queryable voting data.
/// </summary>
public enum ContestStagesFilter
{
    /// <summary>
    ///     Filters the queryable voting data to points awarded in all contest stages (i.e. no filter).
    /// </summary>
    All,

    /// <summary>
    ///     Filters the queryable voting data to points awarded in the Grand Final contest stage.
    /// </summary>
    GrandFinal,

    /// <summary>
    ///     Filters the queryable voting data to points awarded in the Semi-Final 1 and Semi-Final 2 contest stages.
    /// </summary>
    SemiFinals,

    /// <summary>
    ///     Filters the queryable voting data to points awarded in the Semi-Final 1 contest stage.
    /// </summary>
    SemiFinal1,

    /// <summary>
    ///     Filters the queryable voting data to points awarded in the Semi-Final 2 contest stage.
    /// </summary>
    SemiFinal2,
}
