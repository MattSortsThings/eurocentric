using Eurocentric.Features.AcceptanceTests.TestUtils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;

[Trait("Category", "container")]
[Trait("Category", "acceptance")]
[Collection(nameof(AdminApiV0TestCollection))]
public abstract class AcceptanceTestBase : IDisposable
{
    private readonly WebAppFixture _fixture;

    protected AcceptanceTestBase(WebAppFixture webAppFixture)
    {
        _fixture = webAppFixture;
    }

    private protected ITestClient Client => _fixture;

    private protected abstract int MajorApiVersion { get; }

    private protected abstract int MinorApiVersion { get; }

    public void Dispose()
    {
        _fixture.Reset();
        GC.SuppressFinalize(this);
    }
}
