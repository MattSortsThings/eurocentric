using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utilities;

/// <summary>
///     Sends a REST request to the in-memory web application fixture and returns its response.
/// </summary>
public interface IWebAppFixtureRestClient
{
    /// <summary>
    ///     Sends the REST request via HTTP to the in-memory web application using a new asynchronous service scope and returns
    ///     the response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <param name="cancellationToken">
    ///     A cancellation token to observe while waiting for the asynchronous operation to complete.
    /// </param>
    /// <returns>A task that represents the asynchronous request sending operation. The task result contains the response.</returns>
    public Task<ProblemOrResponse> SendRequestAsync(RestRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sends the REST request via HTTP to the in-memory web application using a new asynchronous service scope and returns
    ///     the response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <param name="cancellationToken">
    ///     A cancellation token to observe while waiting for the asynchronous operation to complete.
    /// </param>
    /// <typeparam name="T">The successful response type.</typeparam>
    /// <returns>A task that represents the asynchronous request sending operation. The task result contains the response.</returns>
    public Task<ProblemOrResponse<T>> SendRequestAsync<T>(RestRequest request, CancellationToken cancellationToken = default);
}
