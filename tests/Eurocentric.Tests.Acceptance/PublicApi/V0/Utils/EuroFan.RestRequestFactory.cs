using RestSharp;

namespace Eurocentric.Tests.Acceptance.PublicApi.V0.Utils;

public abstract partial class EuroFan
{
    private sealed class RestRequestFactory(string apiVersion, string apiKey) : IPublicApiV0RestRequestFactory
    {
        public string ApiVersion { get; } = apiVersion;

        public string ApiKey { get; } = apiKey;

        public RestRequest GetQueryableBroadcasts()
        {
            return new RestRequest("/public/api/{apiVersion}/queryable-broadcasts")
                .AddUrlSegment("apiVersion", ApiVersion)
                .AddHeader("X-Api-Key", ApiKey);
        }

        public RestRequest GetQueryableContests()
        {
            return new RestRequest("/public/api/{apiVersion}/queryable-contests")
                .AddUrlSegment("apiVersion", ApiVersion)
                .AddHeader("X-Api-Key", ApiKey);
        }

        public RestRequest GetQueryableCountries()
        {
            return new RestRequest("/public/api/{apiVersion}/queryable-countries")
                .AddUrlSegment("apiVersion", ApiVersion)
                .AddHeader("X-Api-Key", ApiKey);
        }
    }
}
