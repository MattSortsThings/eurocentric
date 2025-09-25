using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Views;

public sealed record QueryableBroadcast
{
    public DateOnly BroadcastDate { get; init; }

    public int ContestYear { get; init; }

    public ContestStage ContestStage { get; init; }

    public int Competitors { get; init; }

    public int Juries { get; init; }

    public int Televotes { get; init; }
}
