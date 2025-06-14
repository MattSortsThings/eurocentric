namespace Eurocentric.Domain.Abstractions;

/// <summary>
///     Abstract base class for a domain entity type.
/// </summary>
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    ///     Parameterless constructor for EF Core.
    /// </summary>
    private protected Entity() { }

    /// <summary>
    ///     Gets all the domain events to be published by the entity.
    /// </summary>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    ///     Clears the entity's <see cref="DomainEvents" /> list.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    ///     Adds the provided domain event to the entity's <see cref="DomainEvents" /> list.
    /// </summary>
    /// <param name="domainEvent">The domain event to be added.</param>
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
