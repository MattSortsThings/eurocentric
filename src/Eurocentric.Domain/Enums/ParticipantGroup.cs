namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies a participant's group in its contest.
/// </summary>
public enum ParticipantGroup
{
    /// <summary>
    ///     Participant group 0: non-competing participants that award televote points in all broadcasts.
    /// </summary>
    Zero = 0,

    /// <summary>
    ///     Participant group 1: participants that vote in, and may compete in, the Semi-Final 1 and Grand Final broadcasts.
    /// </summary>
    One = 1,

    /// <summary>
    ///     Participant group 2: participants that vote in, and may compete in, the Semi-Final 2 and Grand Final broadcasts.
    /// </summary>
    Two = 2
}
