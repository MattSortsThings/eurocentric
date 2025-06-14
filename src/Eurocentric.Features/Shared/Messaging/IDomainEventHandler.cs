using Eurocentric.Domain.Abstractions;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     An application domain event handler.
/// </summary>
/// <typeparam name="TDomainEvent">The domain event type.</typeparam>
public interface IDomainEventHandler<in TDomainEvent> : IConsumer<TDomainEvent>
    where TDomainEvent : IDomainEvent;
