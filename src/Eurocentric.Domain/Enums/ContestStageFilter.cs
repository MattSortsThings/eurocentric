namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies the contest stage filter to be applied to the queryable voting data.
/// </summary>
public enum ContestStageFilters
{
    /// <summary>
    ///     Filters the queryable voting data to points awarded in any contest stage; this is equivalent to no contest stage
    ///     filter.
    /// </summary>
    Any,

    /// <summary>
    ///     Filters the queryable voting data to points awarded in the Grand Final contest stage only.
    /// </summary>
    GrandFinal,

    /// <summary>
    ///     Filters the queryable voting data to points awarded in the Semi-Final 1 and Semi-Final 2 contest stages only.
    /// </summary>
    SemiFinals,

    /// <summary>
    ///     Filters the queryable voting data to points awarded in the Semi-Final 1 contest stage only.
    /// </summary>
    SemiFinal1,

    /// <summary>
    ///     Filters the queryable voting data to points awarded in the Semi-Final 2 contest stage only.
    /// </summary>
    SemiFinal2,
}
