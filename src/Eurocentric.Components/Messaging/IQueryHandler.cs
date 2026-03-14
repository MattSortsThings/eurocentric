using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     Handles an internal query that <i>either</i> succeeds, reads data from the system, and returns a
///     <typeparamref name="TValue" /> object, <i>or</i> fails and returns a <see cref="DomainError" /> object.
/// </summary>
/// <typeparam name="TQuery">The query type.</typeparam>
/// <typeparam name="TValue">The successful return value type.</typeparam>
public interface IQueryHandler<in TQuery, TValue> : IRequestHandler<TQuery, Result<TValue, DomainError>>
    where TQuery : class, IQuery<TValue>
    where TValue : class;
