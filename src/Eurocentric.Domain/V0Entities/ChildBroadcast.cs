using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public sealed record ChildBroadcast
{
    public Guid ChildBroadcastId { get; init; }

    public ContestStage ContestStage { get; init; }

    public bool Completed { get; set; }
}
