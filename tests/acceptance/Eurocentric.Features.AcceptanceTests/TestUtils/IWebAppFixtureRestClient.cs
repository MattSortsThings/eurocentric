using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public interface IWebAppFixtureRestClient
{
    /// <summary>
    ///     Sends the REST request via HTTP to the web app fixture using a new asynchronous service scope and returns the
    ///     response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <param name="cancellationToken">
    ///     A cancellation token to observe while waiting for the asynchronous operation to complete.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous request sending operation. The task result contains the response.
    /// </returns>
    Task<BiRestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sends the REST request via HTTP to the in-memory web application using a new asynchronous service scope and returns
    ///     the response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <param name="cancellationToken">
    ///     A cancellation token to observe while waiting for the asynchronous operation to complete.
    /// </param>
    /// <typeparam name="T">The successful response body type.</typeparam>
    /// <returns>
    ///     A task that represents the asynchronous request sending operation. The task result contains the response.
    /// </returns>
    Task<BiRestResponse<T>> SendAsync<T>(RestRequest request, CancellationToken cancellationToken = default)
        where T : class;
}
