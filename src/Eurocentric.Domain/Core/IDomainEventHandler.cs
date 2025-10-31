using SlimMessageBus;

namespace Eurocentric.Domain.Core;

/// <summary>
///     A handler for a domain event.
/// </summary>
/// <typeparam name="T">The domain event type.</typeparam>
public interface IDomainEventHandler<in T> : IConsumer<T>
    where T : IDomainEvent;
