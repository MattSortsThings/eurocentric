using Eurocentric.Apis.Public.V0.Enums;

namespace Eurocentric.Apis.Public.V0.Dtos.Scoreboards;

public sealed record ScoreboardMetadata
{
    public int ContestYear { get; init; }

    public ContestStage ContestStage { get; init; }

    public bool TelevoteOnlyBroadcast { get; init; }
}
