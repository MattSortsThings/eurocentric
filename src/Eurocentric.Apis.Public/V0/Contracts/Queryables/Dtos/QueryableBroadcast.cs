using Eurocentric.Apis.Public.V0.Contracts.Dtos;

namespace Eurocentric.Apis.Public.V0.Contracts.Queryables.Dtos;

public sealed record QueryableBroadcast
{
    public DateOnly BroadcastDate { get; init; }

    public int ContestYear { get; init; }

    public ContestStage ContestStage { get; init; }

    public int Competitors { get; init; }

    public int Juries { get; init; }

    public int Televotes { get; init; }
}
