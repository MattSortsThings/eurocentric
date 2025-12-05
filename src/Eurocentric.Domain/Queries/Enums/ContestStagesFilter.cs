namespace Eurocentric.Domain.Queries.Enums;

/// <summary>
///     Specifies the contest stages filter to be applied to the queried voting data.
/// </summary>
public enum ContestStagesFilter
{
    /// <summary>
    ///     Filters the queried voting data to points awarded in all contest stages (i.e. no filter).
    /// </summary>
    All,

    /// <summary>
    ///     Filters the queried voting data to points awarded in Grand Final broadcasts only.
    /// </summary>
    GrandFinal,

    /// <summary>
    ///     Filters the queried voting data to points awarded in First Semi-Final broadcasts only.
    /// </summary>
    FirstSemiFinal,

    /// <summary>
    ///     Filters the queried voting data to points awarded in Second Semi-Final broadcasts only.
    /// </summary>
    SecondSemiFinal,

    /// <summary>
    ///     Filters the queried voting data to points awarded in First Semi-Final and Second Semi-Final broadcasts only.
    /// </summary>
    BothSemiFinals,
}
