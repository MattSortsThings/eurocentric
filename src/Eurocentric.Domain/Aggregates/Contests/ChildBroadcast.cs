using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

public sealed class ChildBroadcast : Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private ChildBroadcast() { }

    internal ChildBroadcast(BroadcastId childBroadcastId, ContestStage contestStage)
    {
        ChildBroadcastId = childBroadcastId;
        ContestStage = contestStage;
    }

    public BroadcastId ChildBroadcastId { get; private init; } = null!;

    public ContestStage ContestStage { get; private init; }

    public bool Completed { get; internal set; }
}
