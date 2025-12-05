using CSharpFunctionalExtensions;
using SlimMessageBus;

namespace Eurocentric.Domain.Core;

/// <summary>
///     Handles a command that <i>either</i> succeeds and returns a value <i>or</i> fails and returns an error.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
/// <typeparam name="TValue">The successful return value type.</typeparam>
public interface ICommandHandler<in TCommand, TValue> : IRequestHandler<TCommand, Result<TValue, IError>>
    where TCommand : ICommand<TValue>;
