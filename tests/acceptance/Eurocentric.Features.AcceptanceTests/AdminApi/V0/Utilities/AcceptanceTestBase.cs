using Eurocentric.Features.AcceptanceTests.Utilities;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utilities;

[Trait("Category", "container")]
[Trait("Category", "acceptance")]
[Trait("Feature Scope", "admin-api")]
[Collection(nameof(AdminApiV0FeaturesTestCollection))]
public abstract class AcceptanceTestBase(WebAppFixture fixture) : IDisposable
{
    private protected IWebAppFixtureRestClient SutRestClient { get; } = fixture;

    private protected IWebAppFixtureBackDoor SutBackDoor { get; } = fixture;

    public void Dispose()
    {
        fixture.Reset();
        GC.SuppressFinalize(this);
    }
}
