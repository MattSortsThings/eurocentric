using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

[AcceptanceTest]
[Collection(nameof(CleanWebAppTestCollection))]
public abstract class AcceptanceTest : IDisposable
{
    private readonly CleanWebAppFixture _fixture;

    protected AcceptanceTest(CleanWebAppFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    {
        _fixture.Reset();
        GC.SuppressFinalize(this);
    }

    private protected async Task<RestResponse<T>> SendAsync<T>(RestRequest request) => await _fixture.SendAsync<T>(request);

    private protected static RestRequest Get(string resource) =>
        new RestRequest(resource)
            .AddHeader("Accept", "application/json");

    private protected static RestRequest Post(string resource) =>
        new RestRequest(resource, Method.Post)
            .AddHeader("Accept", "application/json")
            .AddHeader("Content-Type", "application/json");
}
