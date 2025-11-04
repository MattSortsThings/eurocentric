using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;

public sealed partial class EuroFanKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.IVotingCountryRankingsEndpoints
    {
        public RestRequest GetVotingCountryPointsAverageRankings(IReadOnlyDictionary<string, object?> queryParams)
        {
            return GetRequest("/public/api/{apiVersion}/voting-country-rankings/points-average")
                .UseDemoApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddQueryParameters(queryParams);
        }
    }
}
