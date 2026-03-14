using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     An internal command that <i>either</i> succeeds, changes the system state, and returns a
///     <typeparamref name="TValue" /> object, <i>or</i> fails and returns a <see cref="DomainError" /> object.
/// </summary>
/// <typeparam name="TValue">The successful return value type.</typeparam>
public interface ICommand<TValue> : IRequest<Result<TValue, DomainError>>
    where TValue : class;
