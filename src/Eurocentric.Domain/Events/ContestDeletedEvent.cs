using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Aggregates.Contests;

namespace Eurocentric.Domain.Events;

/// <summary>
///     A domain event that is raised when a contest aggregate is marked for deletion.
/// </summary>
/// <param name="Contest">The deleted contest aggregate.</param>
public sealed record ContestDeletedEvent(Contest Contest) : IDomainEvent;
