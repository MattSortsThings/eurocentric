using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public sealed partial class RestRequestFactory : IRestRequestFactory.ICompetingCountryRankingsEndpoints
{
    public RestRequest GetCompetingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("public/api/{apiVersion}/rankings/competing-countries/points-average")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);

    public RestRequest GetCompetingCountryPointsConsensusRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("public/api/{apiVersion}/rankings/competing-countries/points-consensus")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);

    public RestRequest GetCompetingCountryPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("public/api/{apiVersion}/rankings/competing-countries/points-in-range")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);

    public RestRequest GetCompetingCountryPointsShareRankings(IReadOnlyDictionary<string, object?> queryParams) =>
        GetRequest("public/api/{apiVersion}/rankings/competing-countries/points-share")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);
}
