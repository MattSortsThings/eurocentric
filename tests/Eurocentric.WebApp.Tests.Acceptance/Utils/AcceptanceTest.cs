using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

[Trait("Category", "DatabaseTest")]
[Trait("Category", "AcceptanceTest")]
[Collection(nameof(CleanWebAppTestCollection))]
public abstract class AcceptanceTest(CleanWebAppFixture fixture) : IDisposable
{
    public void Dispose()
    {
        fixture.Reset();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Sends the request to the web app fixture and returns its response, with the content deserialized into a result
    ///     object.
    /// </summary>
    /// <param name="request">The request to be sent to the web app fixture.</param>
    /// <typeparam name="T">The expected result type.</typeparam>
    /// <returns>A completed task containing the response.</returns>
    private protected async Task<RestResponse<T>> SendAsync<T>(RestRequest request) => await fixture.SendAsync<T>(request);

    /// <summary>
    ///     Sends the request to the web app fixture and returns its response, with the raw text content.
    /// </summary>
    /// <param name="request">The request to be sent to the web app fixture.</param>
    /// <returns>A completed task containing the response.</returns>
    private protected async Task<RestResponse> SendAsync(RestRequest request) => await fixture.SendAsync(request);

    /// <summary>
    ///     Creates a new GET request to the specified resource, with a pre-filled "Accept" header.
    /// </summary>
    /// <param name="resource">The resource path.</param>
    /// <returns>A new <see cref="RestRequest" /> instance, which can be configured further.</returns>
    private protected static RestRequest Get(string resource) => new RestRequest(resource)
        .AddHeader("Accept", "application/json, application/problem+json");

    /// <summary>
    ///     Creates a new POST request to the specified resource, with pre-filled "Accept" and "Content-Type" headers.
    /// </summary>
    /// <param name="resource">The resource path.</param>
    /// <returns>A new <see cref="RestRequest" /> instance, which can be configured further.</returns>
    private protected static RestRequest Post(string resource) => new RestRequest(resource, Method.Post)
        .AddHeader("Accept", "application/json, application/problem+json")
        .AddHeader("Content-Type", "application/json");
}
