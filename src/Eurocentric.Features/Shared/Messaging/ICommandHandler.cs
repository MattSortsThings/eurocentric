using ErrorOr;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     An application command handler that <i>EITHER</i> fails and returns a list of <see cref="Error" /> objects
///     <i>OR</i> succeeds and returns a response of type <typeparamref name="TResponse" />.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
/// <typeparam name="TResponse">The successful response type.</typeparam>
internal interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, ErrorOr<TResponse>>
    where TCommand : ICommand<TResponse>;
