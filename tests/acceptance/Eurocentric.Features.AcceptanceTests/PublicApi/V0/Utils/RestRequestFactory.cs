using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

public sealed class RestRequestFactory : IRestRequestFactory,
    IRestRequestFactory.IQueryablesEndpoints,
    IRestRequestFactory.IRankingsEndpoints
{
    private readonly string _apiVersion;

    public RestRequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public RestRequest GetQueryableContestStages() => GetRequest("/public/api/{apiVersion}/queryables/contest-stages")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetQueryableCountries() => GetRequest("/public/api/{apiVersion}/queryables/countries")
        .AddUrlSegment("apiVersion", _apiVersion);

    public RestRequest GetCompetingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("/public/api/{apiVersion}/rankings/competing-countries/points-average")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);

    public RestRequest GetCompetingCountryPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("/public/api/{apiVersion}/rankings/competing-countries/points-in-range")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);

    public IRestRequestFactory.IQueryablesEndpoints Queryables => this;

    public IRestRequestFactory.IRankingsEndpoints Rankings => this;

    private static RestRequest GetRequest(string route) => new(route);
}
