using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public sealed record ChildBroadcast
{
    public required Guid BroadcastId { get; init; }

    public required ContestStage ContestStage { get; init; }

    public required bool Completed { get; init; }
}
