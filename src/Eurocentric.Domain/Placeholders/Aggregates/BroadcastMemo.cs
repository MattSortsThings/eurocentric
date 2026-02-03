using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders.Aggregates;

public sealed class BroadcastMemo
{
    public required Guid BroadcastId { get; init; }

    public required ContestStage ContestStage { get; init; }

    public required bool Completed { get; init; }
}
