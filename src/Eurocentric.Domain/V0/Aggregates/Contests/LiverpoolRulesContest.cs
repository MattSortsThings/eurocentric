using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Aggregates.Contests;

/// <summary>
///     Represents a "Liverpool" rules contest.
/// </summary>
public sealed record LiverpoolRulesContest : Contest
{
    /// <inheritdoc />
    public override ContestRules ContestRules { get; init; } = ContestRules.Liverpool;
}
