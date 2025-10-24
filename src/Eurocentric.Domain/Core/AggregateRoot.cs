using JetBrains.Annotations;

namespace Eurocentric.Domain.Core;

/// <summary>
///     Abstract base class for a domain aggregate root entity.
/// </summary>
public abstract class AggregateRoot<TId> : Entity
    where TId : GuidAtomicValueObject
{
    [UsedImplicitly(Reason = "EF Core")]
    private protected AggregateRoot() { }

    private protected AggregateRoot(TId id)
    {
        Id = id;
    }

    /// <summary>
    ///     Gets the aggregate's system identifier.
    /// </summary>
    public TId Id { get; private protected init; } = null!;
}
