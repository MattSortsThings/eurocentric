using CSharpFunctionalExtensions;
using Eurocentric.Domain.Errors;
using SlimMessageBus;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     Handler for an internal command that *either* successfully changes the system state and returns a value
///     *or* fails and returns a domain error.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
/// <typeparam name="TValue">The successful command return value type.</typeparam>
public interface ICommandHandler<in TCommand, TValue> : IRequestHandler<TCommand, Result<TValue, DomainError>>
    where TCommand : IRequest<Result<TValue, DomainError>>
    where TValue : struct;
