namespace Eurocentric.Domain.Enums;

/// <summary>
///     Filters the voting data by contest stage.
/// </summary>
public enum ContestStageFilter
{
    /// <summary>
    ///     Any contest stage (i.e. no filter applied).
    /// </summary>
    Any = 0,

    /// <summary>
    ///     First Semi-Finals only.
    /// </summary>
    SemiFinal1 = 1,

    /// <summary>
    ///     Second Semi-Finals only.
    /// </summary>
    SemiFinal2 = 2,

    /// <summary>
    ///     First and Second Semi-Finals only.
    /// </summary>
    SemiFinals = 3,

    /// <summary>
    ///     Grand Finals only.
    /// </summary>
    GrandFinal = 4
}
