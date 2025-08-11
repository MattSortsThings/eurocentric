using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Aggregates.Contests;

namespace Eurocentric.Domain.Events;

/// <summary>
///     A domain event that is raised when a contest aggregate is completed.
/// </summary>
/// <param name="Contest">The created contest aggregate.</param>
public sealed record ContestCreatedEvent(Contest Contest) : IDomainEvent;
