using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;

public sealed partial class EuroFanKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.IListingsEndpoints
    {
        public RestRequest GetCompetingCountryPointsListings(IReadOnlyDictionary<string, object?> queryParams)
        {
            return GetRequest("/public/api/{apiVersion}/listings/competing-country-points")
                .UseDemoApiKey()
                .AddUrlSegment("apiVersion", apiVersion)
                .AddQueryParameters(queryParams);
        }
    }
}
