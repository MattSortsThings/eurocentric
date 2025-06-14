using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Broadcasts;

namespace Eurocentric.Domain.Events;

public sealed record BroadcastDeletedEvent(Broadcast Broadcast) : IDomainEvent;
