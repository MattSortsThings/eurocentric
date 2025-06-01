using ErrorOr;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     An application command that <i>EITHER</i> succeeds and returns a response <i>OR</i> fails and returns a list of
///     errors.
/// </summary>
/// <typeparam name="TResponse">The successful response type.</typeparam>
public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>;
