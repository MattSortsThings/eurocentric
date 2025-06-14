using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Contests;

namespace Eurocentric.Domain.Events;

public sealed record ContestDeletedEvent(Contest Contest) : IDomainEvent;
