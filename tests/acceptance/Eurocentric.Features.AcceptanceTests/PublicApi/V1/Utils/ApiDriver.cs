using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public sealed class ApiDriver : IApiDriver
{
    private readonly RestRequestFactory _requestFactory;
    private readonly SeededWebAppFixture _webAppFixture;

    private ApiDriver(SeededWebAppFixture webAppFixture, RestRequestFactory requestFactory)
    {
        _requestFactory = requestFactory;
        _webAppFixture = webAppFixture;
    }

    public IWebAppFixtureRestClient RestClient => _webAppFixture;

    public IRestRequestFactory RequestFactory => _requestFactory;

    public static ApiDriver Create(SeededWebAppFixture webAppFixture, string apiVersion = "v1.x") =>
        new(webAppFixture, new RestRequestFactory(apiVersion));
}
