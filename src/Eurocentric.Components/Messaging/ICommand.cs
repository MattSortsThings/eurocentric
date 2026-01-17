using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     An internal command that <i>either</i> succeeds and returns a command result value <i>or</i> fails and
///     returns a domain error object.
/// </summary>
/// <typeparam name="TCommandResult">The successful command result type.</typeparam>
public interface ICommand<TCommandResult> : IRequest<Result<TCommandResult, DomainError>>
    where TCommandResult : struct;
