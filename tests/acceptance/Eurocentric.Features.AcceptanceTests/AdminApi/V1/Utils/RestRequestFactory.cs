using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Contests.CreateContest;
using Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public sealed class RestRequestFactory : IRestRequestFactory,
    IRestRequestFactory.IBroadcastsEndpoints,
    IRestRequestFactory.IContestsEndpoints,
    IRestRequestFactory.ICountriesEndpoints
{
    private readonly string _apiVersion;

    public RestRequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public RestRequest GetBroadcast(Guid broadcastId) => GetRequest("/admin/api/{apiVersion}/broadcasts/{broadcastId}")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddUrlSegment("broadcastId", broadcastId);

    public RestRequest CreateContest(CreateContestRequest requestBody) => PostRequest("/admin/api/{apiVersion}/contests")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddJsonBody(requestBody);

    public RestRequest GetContest(Guid contestId) => GetRequest("/admin/api/{apiVersion}/contests/{contestId}")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddUrlSegment("contestId", contestId);

    public RestRequest GetContests() => GetRequest("/admin/api/{apiVersion}/contests")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest CreateCountry(CreateCountryRequest requestBody) => PostRequest("/admin/api/{apiVersion}/countries")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddJsonBody(requestBody);

    public RestRequest GetCountries() => GetRequest("/admin/api/{apiVersion}/countries")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetCountry(Guid countryId) => GetRequest("/admin/api/{apiVersion}/countries/{countryId}")
        .AddUrlSegment("apiVersion", _apiVersion)
        .AddUrlSegment("countryId", countryId);

    public IRestRequestFactory.IBroadcastsEndpoints Broadcasts => this;

    public IRestRequestFactory.IContestsEndpoints Contests => this;

    public IRestRequestFactory.ICountriesEndpoints Countries => this;

    private static RestRequest GetRequest(string route) => new RestRequest(route).UseSecretApiKey();

    private static RestRequest PostRequest(string route) => new RestRequest(route, Method.Post).UseSecretApiKey();
}
