using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public sealed class BroadcastMemo
{
    public Guid BroadcastId { get; init; }

    public ContestStage ContestStage { get; init; }

    public bool Completed { get; init; }
}
