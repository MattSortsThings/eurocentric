using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utils;

[Trait("Category", "Container")]
[Trait("Category", "Acceptance")]
[Collection(nameof(WebAppFixtureTestCollection))]
public abstract class AcceptanceTestBase(WebAppFixture webAppFixture) : IDisposable
{
    private protected ITestHttpClient Sut => webAppFixture;

    public void Dispose()
    {
        webAppFixture?.Reset();
        GC.SuppressFinalize(this);
    }

    private protected static RestRequest Get(string route) => new RestRequest(route)
        .AddHeader("Accept", "application/json, application/problem+json");

    private protected static RestRequest Post(string route) => new RestRequest(route, Method.Post)
        .AddHeader("Accept", "application/json, application/problem+json");
}
