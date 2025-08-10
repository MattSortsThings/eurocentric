using JetBrains.Annotations;

namespace Eurocentric.Domain.Abstractions;

/// <summary>
///     Abstract base class for a domain entity type.
/// </summary>
public abstract class Entity
{
    private List<IDomainEvent>? _domainEvents;

    [UsedImplicitly(Reason = "EF Core")]
    private protected Entity() { }

    internal void AddDomainEvent(IDomainEvent domainEvent)
    {
        if (_domainEvents is null)
        {
            _domainEvents = [domainEvent];
        }
        else
        {
            _domainEvents.Add(domainEvent);
        }
    }

    internal IEnumerable<IDomainEvent> DetachDomainEvents()
    {
        IEnumerable<IDomainEvent> domainEvents = _domainEvents ?? Enumerable.Empty<IDomainEvent>();
        _domainEvents = null;

        return domainEvents;
    }
}
