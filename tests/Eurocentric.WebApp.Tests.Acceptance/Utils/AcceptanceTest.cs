using Eurocentric.TestUtils.Categories;
using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

[AcceptanceTest]
[Collection(nameof(AcceptanceTestCollection))]
public abstract class AcceptanceTest : IDisposable
{
    private readonly WebAppFixture _webAppFixture;

    protected AcceptanceTest(WebAppFixture webAppFixture)
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
