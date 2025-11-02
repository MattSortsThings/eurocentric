using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.Events;

/// <summary>
///     A domain event that is to be published when a <see cref="Broadcast" /> aggregate is deleted.
/// </summary>
/// <param name="Broadcast">The deleted broadcast.</param>
public sealed record BroadcastDeletedEvent(Broadcast Broadcast) : IDomainEvent;
