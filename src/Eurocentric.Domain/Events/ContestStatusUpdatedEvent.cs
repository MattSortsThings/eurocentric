using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Contests;

namespace Eurocentric.Domain.Events;

public sealed record ContestStatusUpdatedEvent(Contest Contest) : IDomainEvent;
