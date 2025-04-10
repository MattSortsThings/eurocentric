using ErrorOr;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     Abstract base class for a request that enters the application pipeline and, once handled, returns <i>either</i> a
///     successful response value <i>or</i> a list of errors.
/// </summary>
/// <typeparam name="TResponse">The successful response type.</typeparam>
public abstract record Request<TResponse> : IRequest<ErrorOr<TResponse>>;
