using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

[Collection(TestCollection.Name)]
[Trait("Category", "container")]
[Trait("Category", "acceptance")]
public abstract class AcceptanceTest(WebAppFixture fixture) : IAsyncLifetime
{
    /// <summary>
    ///     Allows the client to modify the state of the in-memory web app fixture at runtime using scoped operations on the
    ///     fixture's service provider.
    /// </summary>
    private protected IWebAppFixtureBackDoor BackDoor { get; } = fixture;

    /// <summary>
    ///     Sends a REST request to the in-memory web application fixture and returns its response.
    /// </summary>
    private protected IWebAppFixtureRestClient RestClient { get; } = fixture;

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await ValueTask.CompletedTask;
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public async ValueTask InitializeAsync() => await fixture.ResetAsync();
}
