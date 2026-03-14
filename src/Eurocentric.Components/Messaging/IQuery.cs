using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     An internal query that <i>either</i> succeeds, reads data from the system, and returns a
///     <typeparamref name="TValue" /> object, <i>or</i> fails and returns a <see cref="DomainError" /> object.
/// </summary>
/// <typeparam name="TValue">The successful return value type.</typeparam>
public interface IQuery<TValue> : IRequest<Result<TValue, DomainError>>
    where TValue : class;
