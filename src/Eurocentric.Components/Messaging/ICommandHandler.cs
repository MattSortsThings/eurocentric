using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     Handler for an internal command that <i>either</i> succeeds and returns a command result value <i>or</i> fails
///     and returns a domain error object.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
/// <typeparam name="TCommandResult">The successful command result type.</typeparam>
public interface ICommandHandler<in TCommand, TCommandResult>
    : IRequestHandler<TCommand, Result<TCommandResult, DomainError>>
    where TCommandResult : struct
    where TCommand : ICommand<TCommandResult>;
