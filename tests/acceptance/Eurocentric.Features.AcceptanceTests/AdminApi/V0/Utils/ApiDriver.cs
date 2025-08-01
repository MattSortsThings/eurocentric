using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

public sealed class ApiDriver : IApiDriver
{
    private readonly RestRequestFactory _requestFactory;
    private readonly CleanWebAppFixture _webAppFixture;

    private ApiDriver(CleanWebAppFixture webAppFixture, RestRequestFactory requestFactory)
    {
        _webAppFixture = webAppFixture;
        _requestFactory = requestFactory;
    }

    public IWebAppFixtureRestClient RestClient => _webAppFixture;

    public IWebAppFixtureBackDoor BackDoor => _webAppFixture;

    public IRestRequestFactory RequestFactory => _requestFactory;

    public static ApiDriver Create(CleanWebAppFixture webAppFixture, string apiVersion = "v0.0") =>
        new(webAppFixture, new RestRequestFactory(apiVersion));
}
