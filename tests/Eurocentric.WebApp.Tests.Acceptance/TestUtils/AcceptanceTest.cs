using Eurocentric.TestUtils.Categories;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.TestUtils;

[AcceptanceTest]
[Collection(nameof(CleanWebAppTestCollection))]
public abstract class AcceptanceTest : IDisposable
{
    private readonly CleanWebAppFixture _webAppFixture;

    protected AcceptanceTest(CleanWebAppFixture webAppFixture)
    {
        _webAppFixture = webAppFixture;
    }

    private protected RestClient Sut => _webAppFixture.RestClient;

    public void Dispose()
    {
        _webAppFixture.Reset();
        GC.SuppressFinalize(this);
    }
}
