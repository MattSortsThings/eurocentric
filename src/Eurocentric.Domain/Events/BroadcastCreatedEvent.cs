using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.Events;

/// <summary>
///     A domain event that is to be published when a new <see cref="Broadcast" /> aggregate is created.
/// </summary>
/// <param name="Broadcast">The created broadcast.</param>
public sealed record BroadcastCreatedEvent(Broadcast Broadcast) : IDomainEvent;
