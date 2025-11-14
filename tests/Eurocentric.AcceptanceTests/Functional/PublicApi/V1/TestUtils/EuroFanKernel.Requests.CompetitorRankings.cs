using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;

public sealed partial class EuroFanKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.ICompetitorRankingsEndpoints
    {
        public RestRequest GetCompetitorPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams)
        {
            return GetRequest("/public/api/{apiVersion}/competitor-rankings/points-average")
                .UseDemoApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddQueryParameters(queryParams);
        }

        public RestRequest GetCompetitorPointsConsensusRankings(IReadOnlyDictionary<string, object?> queryParams)
        {
            return GetRequest("/public/api/{apiVersion}/competitor-rankings/points-consensus")
                .UseDemoApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddQueryParameters(queryParams);
        }

        public RestRequest GetCompetitorPointsInRangeRankings(IReadOnlyDictionary<string, object?> queryParams)
        {
            return GetRequest("/public/api/{apiVersion}/competitor-rankings/points-in-range")
                .UseDemoApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddQueryParameters(queryParams);
        }

        public RestRequest GetCompetitorPointsShareRankings(IReadOnlyDictionary<string, object?> queryParams)
        {
            return GetRequest("/public/api/{apiVersion}/competitor-rankings/points-share")
                .UseDemoApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddQueryParameters(queryParams);
        }
    }
}
