using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Aggregates.Contests;

/// <summary>
///     Represents a "Stockholm" rules contest.
/// </summary>
public sealed record StockholmRulesContest : Contest
{
    /// <inheritdoc />
    public override ContestRules ContestRules { get; init; } = ContestRules.Stockholm;
}
