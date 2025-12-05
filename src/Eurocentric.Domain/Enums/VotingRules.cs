namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies the voting rules in a broadcast.
/// </summary>
public enum VotingRules
{
    /// <summary>
    ///     The broadcast has televotes and juries.
    /// </summary>
    TelevoteAndJury,

    /// <summary>
    ///     The broadcast has televotes only (no juries).
    /// </summary>
    TelevoteOnly,
}
