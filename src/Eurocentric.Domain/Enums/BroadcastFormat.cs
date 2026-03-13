namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies a broadcast's voting format.
/// </summary>
public enum BroadcastFormat
{
    /// <summary>
    ///     Denotes a broadcast using juries and televotes.
    /// </summary>
    JuryAndTelevote,

    /// <summary>
    ///     Denotes a broadcast using televotes but not juries.
    /// </summary>
    TelevoteOnly,
}
