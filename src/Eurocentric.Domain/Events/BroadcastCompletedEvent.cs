using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Aggregates.Broadcasts;

namespace Eurocentric.Domain.Events;

/// <summary>
///     A domain event that is raised when a broadcast aggregate is completed.
/// </summary>
/// <param name="Broadcast">The completed broadcast aggregate.</param>
public sealed record BroadcastCompletedEvent(Broadcast Broadcast) : IDomainEvent;
