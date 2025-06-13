using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Contests;

namespace Eurocentric.Domain.Events;

public sealed record ContestCreatedEvent(Contest Contest) : IDomainEvent;
