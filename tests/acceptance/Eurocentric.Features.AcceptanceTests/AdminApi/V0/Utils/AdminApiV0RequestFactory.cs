using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Contests;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

public class AdminApiV0RequestFactory : IAdminApiV0RequestFactory, IAdminApiV0RequestFactory.IContestsEndpoints
{
    private readonly string _apiVersion;

    public AdminApiV0RequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public IAdminApiV0RequestFactory.IContestsEndpoints Contests => this;

    public RestRequest CreateContest(CreateContestRequest requestBody) => Post("/admin/api/{apiVersion}/contests")
        .AddJsonBody(requestBody);

    public RestRequest GetContest(Guid contestId) => Get("/admin/api/{apiVersion}/contests/{contestId}")
        .AddUrlSegment("contestId", contestId);

    public RestRequest GetContests() => Get("/admin/api/{apiVersion}/contests");

    private RestRequest Get(string route) => new RestRequest(route)
        .UseSecretApiKey()
        .AddUrlSegment("apiVersion", _apiVersion);

    private RestRequest Post(string route) => new RestRequest(route, Method.Post)
        .UseSecretApiKey()
        .AddUrlSegment("apiVersion", _apiVersion);
}
