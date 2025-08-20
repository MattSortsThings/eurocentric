using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public sealed partial class RestRequestFactory : IRestRequestFactory.IQueryablesEndpoints
{
    public RestRequest GetQueryableBroadcasts() => GetRequest("public/api/{apiVersion}/queryables/broadcasts")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetQueryableContestStages() => GetRequest("public/api/{apiVersion}/queryables/contest-stages")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetQueryableContests() => GetRequest("public/api/{apiVersion}/queryables/contests")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetQueryableCountries() => GetRequest("public/api/{apiVersion}/queryables/countries")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetQueryableVotingMethods() => GetRequest("public/api/{apiVersion}/queryables/voting-methods")
        .AddUrlSegment("apiVersion", _apiVersion);
}
