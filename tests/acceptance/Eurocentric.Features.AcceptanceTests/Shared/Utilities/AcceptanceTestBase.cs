using Eurocentric.Features.AcceptanceTests.Utilities;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Utilities;

[Trait("Category", "container")]
[Trait("Category", "acceptance")]
[Trait("Feature Scope", "shared")]
[Collection(nameof(SharedFeaturesTestCollection))]
public abstract class AcceptanceTestBase(WebAppFixture fixture) : IDisposable
{
    /// <summary>
    ///     Gets a REST client for the system under test.
    /// </summary>
    private protected IWebAppFixtureRestClient SutRestClient { get; } = fixture;

    public void Dispose()
    {
        fixture.Reset();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Initializes a new <see cref="RestRequest" /> object representing an HTTP POST request to the provided route, using
    ///     the web app fixture.
    /// </summary>
    /// <param name="route">The route for the request.</param>
    /// <returns>A new <see cref="RestRequest" /> instance.</returns>
    private protected static RestRequest Post(string route) => new(route, Method.Post);

    /// <summary>
    ///     Initializes a new <see cref="RestRequest" /> object representing an HTTP GET request to the provided route, using
    ///     the web app fixture.
    /// </summary>
    /// <param name="route">The route for the request.</param>
    /// <returns>A new <see cref="RestRequest" /> instance.</returns>
    private protected static RestRequest Get(string route) => new(route);
}
