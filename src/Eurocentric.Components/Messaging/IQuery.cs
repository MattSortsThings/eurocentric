using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     An internal query that <i>either</i> successfully executes a read-only query on the system and returns a value
///     <i>or</i> fails and returns a domain error.
/// </summary>
/// <typeparam name="TValue">The successful query return value type.</typeparam>
public interface IQuery<TValue> : IRequest<Result<TValue, DomainError>>
    where TValue : struct;
