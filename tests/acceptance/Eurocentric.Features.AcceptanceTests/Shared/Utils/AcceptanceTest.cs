using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

[Collection(TestCollection.Name)]
[Trait("Category", "container")]
[Trait("Category", "acceptance")]
public abstract class AcceptanceTest(WebAppFixture fixture) : IAsyncLifetime
{
    private protected IWebAppFixtureBackDoor BackDoor { get; } = fixture;

    private protected IWebAppFixtureRestClient RestClient { get; } = fixture;

    public async ValueTask DisposeAsync()
    {
        await ValueTask.CompletedTask;
        GC.SuppressFinalize(this);
    }

    public async ValueTask InitializeAsync()
    {
        await BackDoor.ExecuteScopedAsync(BackDoorOperations.EnsureDatabaseUnpausedAsync);
        await fixture.ResetAsync();
    }

    private protected static RestRequest Get(string url) => new(url);

    private protected static RestRequest Post(string url) => new(url, Method.Post);
}
