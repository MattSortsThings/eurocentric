using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public sealed class AdminApiV1RequestFactory : IAdminApiV1RequestFactory, IAdminApiV1RequestFactory.ICountriesEndpoints
{
    private readonly string _apiVersion;

    public AdminApiV1RequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public IAdminApiV1RequestFactory.ICountriesEndpoints Countries => this;

    public RestRequest GetCountries() => Get("/admin/api/{apiVersion}/countries");

    public RestRequest GetCountry(Guid countryId) => Get("/admin/api/{apiVersion}/countries/{countryId}")
        .AddUrlSegment("countryId", countryId);

    private RestRequest Get(string route) => new RestRequest(route)
        .UseSecretApiKey()
        .AddUrlSegment("apiVersion", _apiVersion);
}
