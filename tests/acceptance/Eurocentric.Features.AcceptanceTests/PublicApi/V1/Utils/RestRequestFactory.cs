using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public sealed class RestRequestFactory : IRestRequestFactory,
    IRestRequestFactory.IQueryablesEndpoints
{
    private readonly string _apiVersion;

    public RestRequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public RestRequest GetQueryableBroadcasts() => GetRequest("/public/api/{apiVersion}/queryables/broadcasts")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetQueryableContestStages() => GetRequest("/public/api/{apiVersion}/queryables/contest-stages")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetQueryableContests() => GetRequest("/public/api/{apiVersion}/queryables/contests")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetQueryableCountries() => GetRequest("/public/api/{apiVersion}/queryables/countries")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetQueryableVotingMethods() => GetRequest("/public/api/{apiVersion}/queryables/voting-methods")
        .AddUrlSegment("apiVersion", _apiVersion);

    public IRestRequestFactory.IQueryablesEndpoints Queryables => this;

    private static RestRequest GetRequest(string route) => new RestRequest(route).UseDemoApiKey();
}
