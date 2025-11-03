using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;

public sealed partial class EuroFanKernel
{
    private sealed partial class RestRequestFactory : IRestRequestFactory.IQueryablesEndpoints
    {
        public RestRequest GetQueryableCountries() =>
            GetRequest("/public/api/{apiVersion}/queryables/countries")
                .UseDemoApiKey()
                .AddUrlSegment("apiVersion", apiVersion);
    }
}
