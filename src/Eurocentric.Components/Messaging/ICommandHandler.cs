using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     Handles an internal command that <i>either</i> succeeds, changes the system state, and returns a
///     <typeparamref name="TValue" /> object, <i>or</i> fails and returns a <see cref="DomainError" /> object.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
/// <typeparam name="TValue">The successful return value type.</typeparam>
public interface ICommandHandler<in TCommand, TValue> : IRequestHandler<TCommand, Result<TValue, DomainError>>
    where TCommand : class, ICommand<TValue>
    where TValue : class;
