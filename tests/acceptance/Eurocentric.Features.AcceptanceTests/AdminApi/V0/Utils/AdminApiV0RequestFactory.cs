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

    public RestRequest CreateContest(CreateContestRequest requestBody)
    {
        RestRequest restRequest = new("/admin/api/{apiVersion}/contests", Method.Post);

        restRequest.AddUrlSegment("apiVersion", _apiVersion)
            .AddJsonBody(requestBody);

        return restRequest;
    }

    public RestRequest GetContest(Guid contestId)
    {
        RestRequest restRequest = new("/admin/api/{apiVersion}/contests/{contestId}");

        restRequest.AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("contestId", contestId);

        return restRequest;
    }

    public RestRequest GetContests()
    {
        RestRequest restRequest = new("/admin/api/{apiVersion}/contests");

        restRequest.AddUrlSegment("apiVersion", _apiVersion);

        return restRequest;
    }
}
