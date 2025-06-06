using Eurocentric.Features.AcceptanceTests.Utilities;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

public sealed partial class AdminApiV1Driver : IAdminApiV1Driver
{
    private readonly string _apiVersion;
    private readonly IWebAppFixtureRestClient _restClient;

    private AdminApiV1Driver(IWebAppFixtureRestClient restClient, string apiVersion)
    {
        _apiVersion = apiVersion;
        _restClient = restClient;
    }

    public IAdminApiV1Driver.IContests Contests => this;

    public IAdminApiV1Driver.ICountries Countries => this;

    public static AdminApiV1Driver Create(IWebAppFixtureRestClient restClient, string apiVersion) => new(restClient, apiVersion);

    private static RestRequest Get(string route) => new(route);

    private static RestRequest Post(string route) => new(route, Method.Post);
}
