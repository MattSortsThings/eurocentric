using ErrorOr;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     An application query that <i>EITHER</i> fails and returns a list of errors <i>OR</i> succeeds and returns a
///     response.
/// </summary>
/// <typeparam name="TResponse">The successful response type.</typeparam>
internal interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>;
