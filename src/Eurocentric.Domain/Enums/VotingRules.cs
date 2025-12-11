namespace Eurocentric.Domain.Enums;

/// <summary>
///     Specifies a broadcast's voting rules.
/// </summary>
public enum VotingRules
{
    /// <summary>
    ///     The broadcast uses televotes and juries.
    /// </summary>
    TelevoteAndJury,

    /// <summary>
    ///     The broadcast uses televotes only.
    /// </summary>
    TelevoteOnly,
}
