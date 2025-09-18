using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Analytics.Scoreboard;

public sealed record ScoreboardMetadata
{
    public int ContestYear { get; init; }

    public ContestStage ContestStage { get; init; }

    public bool TelevoteOnlyBroadcast { get; init; }
}
