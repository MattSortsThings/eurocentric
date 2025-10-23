using CSharpFunctionalExtensions;
using SlimMessageBus;

namespace Eurocentric.Domain.Core;

/// <summary>
///     A handler for a query that <i>either</i> succeeds and returns a value <i>or</i> fails and returns an error.
/// </summary>
/// <typeparam name="TQuery">The query type.</typeparam>
/// <typeparam name="TValue">The successful return value type.</typeparam>
public interface IQueryHandler<in TQuery, TValue> : IRequestHandler<TQuery, Result<TValue, IDomainError>>
    where TQuery : IQuery<TValue>;
