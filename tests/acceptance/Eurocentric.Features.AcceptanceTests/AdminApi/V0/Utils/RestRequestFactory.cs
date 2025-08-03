using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Contests.CreateContest;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

public sealed class RestRequestFactory : IRestRequestFactory, IRestRequestFactory.IContestsEndpoints
{
    private readonly string _apiVersion;

    public RestRequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public RestRequest CreateContest(CreateContestRequest requestBody) => PostRequest("/admin/api/{apiVersion}/contests")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddJsonBody(requestBody);

    public RestRequest GetContest(Guid contestId) => GetRequest("/admin/api/{apiVersion}/contests/{contestId}")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddUrlSegment("contestId", contestId);

    public RestRequest GetContests() => GetRequest("/admin/api/{apiVersion}/contests")
        .AddUrlSegment("apiVersion", _apiVersion);

    public IRestRequestFactory.IContestsEndpoints Contests => this;


    private static RestRequest GetRequest(string route) => new RestRequest(route).UseSecretApiKey();

    private static RestRequest PostRequest(string route) => new RestRequest(route, Method.Post).UseSecretApiKey();
}
