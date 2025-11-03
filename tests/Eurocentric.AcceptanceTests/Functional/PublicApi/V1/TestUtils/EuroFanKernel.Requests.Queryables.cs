using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;

public sealed partial class EuroFanKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.IQueryablesEndpoints
    {
        public RestRequest GetQueryableContests()
        {
            return GetRequest("/public/api/{apiVersion}/queryables/contests")
                .UseDemoApiKey()
                .AddUrlSegment("apiVersion", apiVersion);
        }

        public RestRequest GetQueryableCountries()
        {
            return GetRequest("/public/api/{apiVersion}/queryables/countries")
                .UseDemoApiKey()
                .AddUrlSegment("apiVersion", apiVersion);
        }
    }
}
