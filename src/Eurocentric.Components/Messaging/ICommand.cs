using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     An internal command that <i>either</i> successfully changes the system state and returns a value
///     <i>or</i> fails and returns a domain error.
/// </summary>
/// <typeparam name="TValue">The successful command return value type.</typeparam>
public interface ICommand<TValue> : IRequest<Result<TValue, DomainError>>
    where TValue : struct;
