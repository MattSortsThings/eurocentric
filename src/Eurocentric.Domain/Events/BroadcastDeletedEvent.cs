using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Aggregates.Broadcasts;

namespace Eurocentric.Domain.Events;

/// <summary>
///     A domain event that is raised when a broadcast aggregate is marked for deletion.
/// </summary>
/// <param name="Broadcast">The deleted broadcast aggregate.</param>
public sealed record BroadcastDeletedEvent(Broadcast Broadcast) : IDomainEvent;
