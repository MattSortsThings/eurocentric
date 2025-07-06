using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.PlaceholderEntities;

public abstract record QueryableAward
{
    public required int ContestYear { get; init; }

    public required ContestStage ContestStage { get; init; }

    public required string BroadcastTag { get; init; }

    public required int RunningOrderPosition { get; init; }

    public required string CompetingCountryCode { get; init; }

    public required string VotingCountryCode { get; init; }

    public required int PointsValue { get; init; }

    public required int MaxPointsValue { get; init; }

    public required double RealPointsValue { get; init; }

    public required double NormalizedPointsValue { get; init; }
}
