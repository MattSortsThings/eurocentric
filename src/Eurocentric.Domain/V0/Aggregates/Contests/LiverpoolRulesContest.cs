using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Aggregates.Contests;

/// <summary>
///     Represents a "Liverpool" rules contest aggregate.
/// </summary>
public sealed record LiverpoolRulesContest : Contest
{
    public override ContestRules ContestRules { get; init; } = ContestRules.Liverpool;
}
