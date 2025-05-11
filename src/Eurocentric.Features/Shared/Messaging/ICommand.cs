using ErrorOr;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     A command that <i>EITHER</i> succeeds and returns a response <i>OR</i> fails and returns a list of errors.
/// </summary>
/// <remarks>
///     A command is an operation that changes the state of the system by creating, updating, or deleting one or more
///     domain aggregates.
/// </remarks>
/// <typeparam name="TResponse">The successful response type.</typeparam>
internal interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>;
