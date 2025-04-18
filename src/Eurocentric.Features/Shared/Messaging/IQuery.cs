using ErrorOr;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     A query that, once handled, returns <i>either</i> a successful response value <i>or</i> a list of errors.
/// </summary>
/// <typeparam name="TResponse">The successful response type.</typeparam>
public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>;
