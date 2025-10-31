namespace Eurocentric.Domain.Core;

/// <summary>
///     An event that is published by a domain entity and may be handled by zero, one or more aggregates in the same
///     domain as part of the same transaction.
/// </summary>
public interface IDomainEvent;
