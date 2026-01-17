using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     An internal query that <i>either</i> succeeds and returns a query result value <i>or</i> fails and returns
///     a domain error object.
/// </summary>
/// <typeparam name="TQueryResult">The successful query result type.</typeparam>
public interface IQuery<TQueryResult> : IRequest<Result<TQueryResult, DomainError>>
    where TQueryResult : struct;
