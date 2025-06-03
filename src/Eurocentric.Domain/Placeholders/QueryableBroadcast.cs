using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders;

public sealed record QueryableBroadcast
{
    public int ContestYear { get; init; }

    public ContestStage ContestStage { get; init; }

    public DateOnly BroadcastDate { get; init; }

    public bool TelevoteOnly { get; init; }
}
