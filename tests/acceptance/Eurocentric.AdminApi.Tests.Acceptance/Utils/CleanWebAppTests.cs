using Eurocentric.Tests.Utils.Attributes;
using Eurocentric.Tests.Utils.Fixtures;

namespace Eurocentric.AdminApi.Tests.Acceptance.Utils;

[DatabaseTest]
[Collection(nameof(CleanWebAppTestCollection))]
public abstract class CleanWebAppTests : IDisposable
{
    private readonly CleanWebAppFixture _fixture;

    protected CleanWebAppTests(CleanWebAppFixture fixture)
    {
        _fixture = fixture;
    }

    private protected ITestableWebApi Sut => _fixture;

    public void Dispose()
    {
        _fixture.Dispose();
        GC.SuppressFinalize(this);
    }
}
