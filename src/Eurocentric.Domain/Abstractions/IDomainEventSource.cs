namespace Eurocentric.Domain.Abstractions;

/// <summary>
///     An entity that emits domain events.
/// </summary>
public interface IDomainEventSource
{
    /// <summary>
    ///     Detaches all the domain events from the aggregate and its owned entities and returns them in an array.
    /// </summary>
    /// <returns>A (possibly empty) array of objects that implement <see cref="IDomainEvent" />.</returns>
    IDomainEvent[] DetachAllDomainEvents();
}
