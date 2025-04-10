using Eurocentric.TestUtils.WebAppFixtures;

namespace Eurocentric.Features.SubcutaneousTests.Utils;

[Trait("Category", "Container")]
[Trait("Category", "Subcutaneous")]
[Collection(nameof(WebAppFixtureTestCollection))]
public abstract class SubcutaneousTestBase(WebAppFixture webAppFixture) : IDisposable
{
    private protected ITestMessageBus Sut => webAppFixture;

    public void Dispose()
    {
        webAppFixture?.Reset();
        GC.SuppressFinalize(this);
    }
}
