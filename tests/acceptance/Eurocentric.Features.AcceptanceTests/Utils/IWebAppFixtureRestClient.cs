using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utils;

/// <summary>
///     Sends a REST request to the in-memory web application fixture and returns its response.
/// </summary>
public interface IWebAppFixtureRestClient
{
    /// <summary>
    ///     Sends the REST request via HTTP to the web app fixture using a new asynchronous service scope and returnsthe
    ///     response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <param name="cancellationToken">
    ///     A cancellation token to observe while waiting for the asynchronous operation to complete.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous request sending operation. The task result contains the response.
    /// </returns>
    public Task<ProblemOrResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sends the REST request via HTTP to the in-memory web application using a new asynchronous service scope and returns
    ///     the response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <param name="cancellationToken">
    ///     A cancellation token to observe while waiting for the asynchronous operation to complete.
    /// </param>
    /// <typeparam name="TResponse">The successful response type.</typeparam>
    /// <returns>
    ///     A task that represents the asynchronous request sending operation. The task result contains the response.
    /// </returns>
    public Task<ProblemOrResponse<TResponse>> SendAsync<TResponse>(RestRequest request,
        CancellationToken cancellationToken = default)
        where TResponse : class;
}
