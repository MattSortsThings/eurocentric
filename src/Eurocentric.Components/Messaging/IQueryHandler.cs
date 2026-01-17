using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     Handler for an internal query that <i>either</i> succeeds and returns a query result value <i>or</i>
///     fails and returns a domain error object.
/// </summary>
/// <typeparam name="TQuery">The query type.</typeparam>
/// <typeparam name="TQueryResult">The successful query result type.</typeparam>
public interface IQueryHandler<in TQuery, TQueryResult> : IRequestHandler<TQuery, Result<TQueryResult, DomainError>>
    where TQueryResult : struct
    where TQuery : IQuery<TQueryResult>;
