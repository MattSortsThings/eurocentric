using Eurocentric.AcceptanceTests.TestUtils;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V0.TestUtils;

public sealed partial class EuroFanKernel
{
    private readonly RestRequestFactory _restRequestFactory;
    private readonly WebAppFixture _webAppFixture;

    private EuroFanKernel(WebAppFixture webAppFixture, RestRequestFactory restRequestFactory)
    {
        _webAppFixture = webAppFixture;
        _restRequestFactory = restRequestFactory;
    }

    public IWebAppFixtureRestClient Client => _webAppFixture;

    public IRestRequestFactory Requests => _restRequestFactory;

    public static EuroFanKernel Create(WebAppFixture webAppFixture, string apiVersion = "v0.x")
    {
        RestRequestFactory restRequestFactory = new(apiVersion);

        return new EuroFanKernel(webAppFixture, restRequestFactory);
    }
}
