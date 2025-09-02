using Eurocentric.Features.AcceptanceTests.TestUtils;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;

public sealed class ApiDriver : IApiDriver
{
    private readonly RestRequestFactory _requestFactory;
    private readonly SeededWebAppFixture _webAppFixture;

    private ApiDriver(SeededWebAppFixture webAppFixture, RestRequestFactory requestFactory)
    {
        _webAppFixture = webAppFixture;
        _requestFactory = requestFactory;
    }

    public IWebAppFixtureRestClient RestClient => _webAppFixture;

    public IWebAppFixtureBackDoor BackDoor => _webAppFixture;

    public IRestRequestFactory RequestFactory => _requestFactory;

    public static ApiDriver Create(SeededWebAppFixture webAppFixture, string apiVersion = "v0.x") =>
        new(webAppFixture, new RestRequestFactory(apiVersion));
}
