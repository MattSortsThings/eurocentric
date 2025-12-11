using CSharpFunctionalExtensions;
using Eurocentric.Domain.Abstractions.Errors;
using SlimMessageBus;

namespace Eurocentric.Domain.Abstractions.Messaging;

/// <summary>
///     A query that <i>either</i> succeeds and returns a value <i>or</i> fails and returns an error.
/// </summary>
/// <typeparam name="TValue">The successful return value type.</typeparam>
public interface IQuery<TValue> : IRequest<Result<TValue, IDomainError>>;
