using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

[Trait("Category", "container")]
[Trait("Category", "acceptance")]
[Collection(TestCollection.Name)]
public abstract class AcceptanceTest(WebAppFixture fixture) : IAsyncLifetime
{
    private protected IWebAppFixtureBackDoor BackDoor { get; } = fixture;

    private protected IWebAppFixtureRestClient RestClient { get; } = fixture;

    public async ValueTask DisposeAsync()
    {
        await Task.CompletedTask;
        GC.SuppressFinalize(this);
    }

    public async ValueTask InitializeAsync() => await fixture.ResetAsync();
}
