using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     Handler for an internal query that <i>either</i> successfully executes a read-only query on the system
///     and returns a value <i>or</i> fails and returns a domain error.
/// </summary>
/// <typeparam name="TQuery">The query type.</typeparam>
/// <typeparam name="TValue">The successful query return value type.</typeparam>
public interface IQueryHandler<in TQuery, TValue> : IRequestHandler<TQuery, Result<TValue, DomainError>>
    where TQuery : IRequest<Result<TValue, DomainError>>
    where TValue : struct;
