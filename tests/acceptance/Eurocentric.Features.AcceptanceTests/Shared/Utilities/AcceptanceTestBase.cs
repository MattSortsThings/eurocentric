using Eurocentric.Features.AcceptanceTests.Utilities;

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
}
