using Eurocentric.AcceptanceTests.TestUtils;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils;

public sealed partial class AdminKernel
{
    private readonly RestRequestFactory _restRequestFactory;
    private readonly WebAppFixture _webAppFixture;

    private AdminKernel(WebAppFixture webAppFixture, RestRequestFactory restRequestFactory)
    {
        _webAppFixture = webAppFixture;
        _restRequestFactory = restRequestFactory;
    }

    public IWebAppFixtureBackDoor BackDoor => _webAppFixture;

    public IWebAppFixtureRestClient Client => _webAppFixture;

    public IRestRequestFactory Requests => _restRequestFactory;

    public static AdminKernel Create(WebAppFixture webAppFixture, string apiVersion = "v0.x")
    {
        RestRequestFactory restRequestFactory = new(apiVersion);

        return new AdminKernel(webAppFixture, restRequestFactory);
    }
}
