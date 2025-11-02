using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.Events;

/// <summary>
///     A domain event that is to be published when a <see cref="Broadcast" /> aggregate is completed.
/// </summary>
/// <param name="Broadcast">The completed broadcast.</param>
public sealed record BroadcastCompletedEvent(Broadcast Broadcast) : IDomainEvent;
