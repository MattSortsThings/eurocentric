using Eurocentric.Features.AcceptanceTests.TestUtils;

namespace Eurocentric.Features.AcceptanceTests.Shared.TestUtils;

[Trait("Category", "container")]
[Trait("Category", "acceptance")]
[Collection(nameof(SharedFeaturesTestCollection))]
public abstract class AcceptanceTestBase : IDisposable
{
    private readonly WebAppFixture _fixture;

    protected AcceptanceTestBase(WebAppFixture webAppFixture)
    {
        _fixture = webAppFixture;
    }

    private protected ITestClient Client => _fixture;

    public void Dispose()
    {
        _fixture.Reset();
        GC.SuppressFinalize(this);
    }
}
