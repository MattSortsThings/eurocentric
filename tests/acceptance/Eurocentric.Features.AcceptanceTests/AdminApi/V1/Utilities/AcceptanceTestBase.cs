using Eurocentric.Features.AcceptanceTests.Utilities;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

[Trait("Category", "container")]
[Trait("Category", "acceptance")]
[Trait("Feature Scope", "admin-api")]
[Collection(nameof(AdminApiV1FeaturesTestCollection))]
public abstract class AcceptanceTestBase(WebAppFixture fixture) : IDisposable
{
    /// <summary>
    ///     Gets a REST client for the system under test.
    /// </summary>
    private protected IWebAppFixtureRestClient SutRestClient { get; } = fixture;

    /// <summary>
    ///     Gets a backdoor for the system under test.
    /// </summary>
    private protected IWebAppFixtureBackDoor SutBackDoor { get; } = fixture;

    public void Dispose()
    {
        fixture.Reset();
        GC.SuppressFinalize(this);
    }
}
