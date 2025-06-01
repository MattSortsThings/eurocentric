using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders;

public sealed record QueryablePointsAward
{
    public int ContestYear { get; init; }

    public ContestStage ContestStage { get; init; }

    public string VotingCountryCode { get; init; } = string.Empty;

    public string CompetingCountryCode { get; init; } = string.Empty;

    public int PointsValue { get; init; }

    public int MaxPointsValue { get; init; }
}
