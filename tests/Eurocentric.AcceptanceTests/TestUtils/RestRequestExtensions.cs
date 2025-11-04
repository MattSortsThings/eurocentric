using RestSharp;

namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     Extension methods for the <see cref="RestRequest" /> class.
/// </summary>
public static class RestRequestExtensions
{
    /// <summary>
    ///     Adds the test DEMO_API_KEY to the REST request as an "X-Api-Key" header.
    /// </summary>
    /// <param name="request">The REST request to be modified.</param>
    /// <returns>The same <see cref="RestRequest" /> instance, so that method invocations can be chained.</returns>
    public static RestRequest UseDemoApiKey(this RestRequest request)
    {
        request.AddHeader("X-Api-Key", TestApiKeys.Demo);

        return request;
    }

    /// <summary>
    ///     Adds the test SECRET_API_KEY to the REST request as an "X-Api-Key" header.
    /// </summary>
    /// <param name="request">The REST request to be modified.</param>
    /// <returns>The same <see cref="RestRequest" /> instance, so that method invocations can be chained.</returns>
    public static RestRequest UseSecretApiKey(this RestRequest request)
    {
        request.AddHeader("X-Api-Key", TestApiKeys.Secret);

        return request;
    }
}
