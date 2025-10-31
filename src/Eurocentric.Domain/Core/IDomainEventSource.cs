namespace Eurocentric.Domain.Core;

/// <summary>
///     A source of domain events.
/// </summary>
public interface IDomainEventSource
{
    /// <summary>
    ///     Detaches all the domain events from this instance and returns them in an array.
    /// </summary>
    /// <returns>A (possibly empty) array of objects that implement <see cref="IDomainEvent" />.</returns>
    IDomainEvent[] DetachAllDomainEvents();
}
