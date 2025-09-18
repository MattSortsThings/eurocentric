using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Analytics.Scoreboard;

public sealed record ScoreboardQuery
{
    public int ContestYear { get; init; }

    public ContestStage ContestStage { get; init; }
}
