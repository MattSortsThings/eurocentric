namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies the voting method used to determine the value of a points award.
/// </summary>
public enum VotingMethod
{
    /// <summary>
    ///     Denotes a points award given by a jury.
    /// </summary>
    Jury,

    /// <summary>
    ///     Denotes a points award given by a televote.
    /// </summary>
    Televote,
}
