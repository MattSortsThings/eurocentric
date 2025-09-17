using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public sealed record StockholmRulesContest : Contest
{
    public override ContestRules ContestRules { get; init; } = ContestRules.Stockholm;
}
