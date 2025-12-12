using Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;
using Eurocentric.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.Tests.Acceptance.Features.AdminApi.V0.Utils.Extensions.Countries;

public static class Requests
{
    extension(IRestRequestFactory factory)
    {
        public RestRequest CreateCountry(CreateCountryRequest requestBody)
        {
            return factory
                .PostRequest("/admin/api/{apiVersion}/countries")
                .AddUrlSegment("apiVersion", factory.ApiVersion)
                .AddApiKeyHeader(factory.ApiKey)
                .AddJsonBody(requestBody);
        }

        public RestRequest DeleteCountry(Guid countryId)
        {
            return factory
                .DeleteRequest("/admin/api/{apiVersion}/countries/{countryId}")
                .AddUrlSegment("apiVersion", factory.ApiVersion)
                .AddUrlSegment("countryId", countryId)
                .AddApiKeyHeader(factory.ApiKey);
        }

        public RestRequest GetCountries()
        {
            return factory
                .GetRequest("/admin/api/{apiVersion}/countries")
                .AddUrlSegment("apiVersion", factory.ApiVersion)
                .AddApiKeyHeader(factory.ApiKey);
        }

        public RestRequest GetCountry(Guid countryId)
        {
            return factory
                .GetRequest("/admin/api/{apiVersion}/countries/{countryId}")
                .AddUrlSegment("apiVersion", factory.ApiVersion)
                .AddUrlSegment("countryId", countryId)
                .AddApiKeyHeader(factory.ApiKey);
        }
    }
}
