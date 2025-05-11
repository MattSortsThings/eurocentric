using Eurocentric.Features.AcceptanceTests.TestUtils;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;

[Trait("Category", "container")]
[Trait("Category", "acceptance")]
public abstract class AcceptanceTestBase
{
    private readonly WebAppFixture _fixture;

    protected AcceptanceTestBase(WebAppFixture webAppFixture)
    {
        _fixture = webAppFixture;
    }

    private protected ITestClient Client => _fixture;

    private protected abstract int MajorApiVersion { get; }

    private protected abstract int MinorApiVersion { get; }
}
