using CSharpFunctionalExtensions;
using SlimMessageBus;

namespace Eurocentric.Domain.Core;

/// <summary>
///     Handles a query that <i>either</i> succeeds and returns a value <i>or</i> fails and returns an error.
/// </summary>
/// <typeparam name="TQuery">The query type.</typeparam>
/// <typeparam name="TValue">The successful return value type.</typeparam>
public interface IQueryHandler<in TQuery, TValue> : IRequestHandler<TQuery, Result<TValue, IError>>
    where TQuery : IQuery<TValue>;
