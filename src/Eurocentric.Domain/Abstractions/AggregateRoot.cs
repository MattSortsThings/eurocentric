namespace Eurocentric.Domain.Abstractions;

/// <summary>
///     Abstract base class for a domain aggregate root entity type.
/// </summary>
/// <typeparam name="TId">The aggregate system ID type.</typeparam>
public abstract class AggregateRoot<TId> : Entity
    where TId : ValueObject
{
    /// <summary>
    ///     Parameterless constructor for EF Core.
    /// </summary>
    private protected AggregateRoot()
    {
    }

    private protected AggregateRoot(TId id)
    {
        Id = id;
    }

    /// <summary>
    ///     Gets the aggregate's system ID.
    /// </summary>
    public TId Id { get; private protected init; } = null!;
}
