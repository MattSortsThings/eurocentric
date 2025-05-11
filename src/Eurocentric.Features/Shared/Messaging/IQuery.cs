using ErrorOr;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     A query that <i>EITHER</i> succeeds and returns a response <i>OR</i> fails and returns a list of errors.
/// </summary>
/// <remarks>A query is a read-only operation that does not change the state of the system.</remarks>
/// <typeparam name="TResponse">The successful response type.</typeparam>
internal interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>;
