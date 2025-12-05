namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies a country's role in a contest.
/// </summary>
public enum ContestRoleType
{
    /// <summary>
    ///     The country is a participant in the contest.
    /// </summary>
    Participant,

    /// <summary>
    ///     The country is a global televote in the contest.
    /// </summary>
    GlobalTelevote,
}
