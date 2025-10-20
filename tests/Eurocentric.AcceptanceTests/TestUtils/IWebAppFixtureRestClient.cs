using RestSharp;

namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     Allows the user to send REST requests to the web application fixture.
/// </summary>
public interface IWebAppFixtureRestClient
{
    /// <summary>
    ///     Asynchronously sends the provided REST request to the web application fixture and returns the response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous send operation. The task result is the REST response.</returns>
    Task<ProblemOrResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously sends the provided REST request to the web application fixture and returns the response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <typeparam name="T">The successful REST response body type.</typeparam>
    /// <returns>A <see cref="Task" /> representing the asynchronous send operation. The task result is the REST response.</returns>
    Task<ProblemOrResponse<T>> SendAsync<T>(RestRequest request, CancellationToken cancellationToken = default)
        where T : class;
}
