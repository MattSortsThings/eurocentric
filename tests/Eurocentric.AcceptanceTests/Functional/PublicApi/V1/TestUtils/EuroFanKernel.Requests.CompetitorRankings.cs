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
    }
}
