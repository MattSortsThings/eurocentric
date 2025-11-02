using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.Events;

/// <summary>
///     A domain event that is to be published when a new <see cref="Contest" /> aggregate is deleted.
/// </summary>
/// <param name="Contest">The deleted contest.</param>
public sealed record ContestDeletedEvent(Contest Contest) : IDomainEvent;
