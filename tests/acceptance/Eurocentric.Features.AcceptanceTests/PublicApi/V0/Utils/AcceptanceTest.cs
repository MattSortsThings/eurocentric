using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

[Trait("Category", "container")]
[Trait("Category", "acceptance")]
[Collection(nameof(PublicApiV0TestCollection))]
public abstract class AcceptanceTest(WebAppFixture fixture) : IAsyncLifetime
{
    private protected IWebAppFixtureBackDoor BackDoor { get; } = fixture;

    private protected IWebAppFixtureRestClient RestClient { get; } = fixture;

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        return ValueTask.CompletedTask;
    }

    public async ValueTask InitializeAsync() => await fixture.ResetAsync();
}
