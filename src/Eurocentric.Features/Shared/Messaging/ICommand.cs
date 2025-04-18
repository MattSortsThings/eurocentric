using ErrorOr;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     A command that, once handled, returns <i>either</i> a successful response value <i>or</i> a list of errors.
/// </summary>
/// <typeparam name="TResponse">The successful response type.</typeparam>
internal interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>;
