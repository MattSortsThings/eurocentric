using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Aggregates.Contests;

/// <summary>
///     Represents a "Stockholm" rules contest aggregate.
/// </summary>
public sealed record StockholmRulesContest : Contest
{
    public override ContestRules ContestRules { get; init; } = ContestRules.Stockholm;
}
