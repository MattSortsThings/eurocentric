using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.Events;

/// <summary>
///     A domain event that is to be published when a new <see cref="Contest" /> aggregate is created.
/// </summary>
/// <param name="Contest">The created contest.</param>
public sealed record ContestCreatedEvent(Contest Contest) : IDomainEvent;
