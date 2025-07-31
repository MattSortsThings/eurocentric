namespace Eurocentric.Features.PublicApi.V0.Common.Enums;

/// <summary>
///     Filters the queryable data by the voting method used to determine the points value.
/// </summary>
public enum QueryableVotingMethod
{
    /// <summary>
    ///     Points awarded using any voting method (i.e. no filter applied).
    /// </summary>
    Any = 0,

    /// <summary>
    ///     Points awarded using jury voting only.
    /// </summary>
    Jury = 1,

    /// <summary>
    ///     Points awarded using televoting only.
    /// </summary>
    Televote = 2
}
