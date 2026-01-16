using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     Allows the user to interact with the test web app using its API endpoints.
/// </summary>
public interface ITestWebAppRestClient
{
    /// <summary>
    ///     Sends the REST request to the test web app and returns the REST response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation. The task's result is the discriminated union
    ///     of <i>either</i> a success REST response with a raw response body <i>or</i> a failure REST response with a
    ///     deserialized <see cref="ProblemDetails" /> response body object.
    /// </returns>
    Task<SuccessOrFailureRestResponse> SendAsync(RestRequest request);

    /// <summary>
    ///     Sends the REST request to the test web app and returns the REST response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <typeparam name="TBody">The successful response body type.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation. The task's result is the discriminated union
    ///     of <i>either</i> a success REST response with a deserialized response body object of type
    ///     <typeparamref name="TBody" /> <i>or</i> a failure REST response with a deserialized
    ///     <see cref="ProblemDetails" /> response body object.
    /// </returns>
    Task<SuccessOrFailureRestResponse<TBody>> SendAsync<TBody>(RestRequest request)
        where TBody : class;

    /// <summary>
    ///     Sends the REST request to the test web app and asserts that it succeeded.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    Task SendSafeAsync(RestRequest request);

    /// <summary>
    ///     Sends the REST request to the test web app,asserts that it succeeded, and returns the deserialized response
    ///     body object.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <typeparam name="TBody">The successful response body type.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation. The task's result is the deserialized
    ///     response body object.
    /// </returns>
    Task<TBody> SendSafeAsync<TBody>(RestRequest request)
        where TBody : class;
}
