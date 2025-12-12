using RestSharp;

namespace Eurocentric.Tests.Acceptance.Utils;

/// <summary>
///     Allows the user to interact with the in-memory web application via its web API surface.
/// </summary>
public interface IWebAppRestClient
{
    /// <summary>
    ///     Sends the REST request to the web application and returns the successful or unsuccessful REST response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous send operation. The task's result is the successful or
    ///     unsuccessful REST response.
    /// </returns>
    Task<UnionRestResponse> SendRequestAsync(RestRequest request);

    /// <summary>
    ///     Sends the REST request to the web application and returns the successful or unsuccessful REST response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <typeparam name="T">The successful response body type.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous send operation. The task's result is the successful or
    ///     unsuccessful REST response.
    /// </returns>
    Task<UnionRestResponse<T>> SendRequestAsync<T>(RestRequest request)
        where T : class;

    /// <summary>
    ///     Sends the REST request to the web application and asserts that it succeeded.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous send operation.</returns>
    Task SendSafeRequestAsync(RestRequest request);

    /// <summary>
    ///     Sends the REST request to the web application, asserts that it succeeded, and sends the response body object.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <typeparam name="T">The successful response body type.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous send operation. The task's result is the response body
    ///     object.
    /// </returns>
    Task<T> SendSafeRequestAsync<T>(RestRequest request)
        where T : class;
}
