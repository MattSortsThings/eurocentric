using Eurocentric.Features.AcceptanceTests.TestUtils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

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

    public static ApiDriver Create(CleanWebAppFixture webAppFixture, string apiVersion = "v1.x") =>
        new(webAppFixture, new RestRequestFactory(apiVersion));
}
