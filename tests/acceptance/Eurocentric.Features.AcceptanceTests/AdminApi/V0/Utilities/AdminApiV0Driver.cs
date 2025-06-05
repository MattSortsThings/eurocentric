using Eurocentric.Features.AcceptanceTests.Utilities;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utilities;

public sealed partial class AdminApiV0Driver : IAdminApiV0Driver
{
    private readonly string _apiVersion;
    private readonly IWebAppFixtureRestClient _restClient;

    private AdminApiV0Driver(IWebAppFixtureRestClient restClient, string apiVersion)
    {
        _apiVersion = apiVersion;
        _restClient = restClient;
    }

    public IAdminApiV0Driver.IContests Contests => this;

    public static AdminApiV0Driver Create(IWebAppFixtureRestClient restClient, string apiVersion) =>
        new(restClient, apiVersion);

    private static RestRequest Get(string route) => new(route);

    private static RestRequest Post(string route) => new(route, Method.Post);
}
