using ErrorOr;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     Abstract base class for a request handler that receives a request, handles it asynchronously, and returns
///     <i>either</i> a successful response value <i>or</i> a list of errors.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The successful response type.</typeparam>
public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, ErrorOr<TResponse>>
{
    /// <summary>
    ///     Asynchronously handles the request and returns <i>either</i> a successful response value <i>or</i> a list of
    ///     errors.
    /// </summary>
    /// <param name="request">The request to be handled.</param>
    /// <param name="cancellationToken">Cancels the operation.</param>
    /// <returns>
    ///     A task representing the asynchronous request handling operation. The task's result is the return value for the
    ///     handled request.
    /// </returns>
    public abstract Task<ErrorOr<TResponse>> OnHandle(TRequest request, CancellationToken cancellationToken = default);
}
