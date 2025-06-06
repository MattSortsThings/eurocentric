using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Countries;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;

public sealed partial class AdminApiV1Driver : IAdminApiV1Driver.ICountries
{
    public async Task<ProblemOrResponse<CreateCountryResponse>> CreateCountry(CreateCountryRequest requestBody,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = Post("/admin/api/{apiVersion}/countries")
            .UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddJsonBody(requestBody);

        return await _restClient.SendRequestAsync<CreateCountryResponse>(request, cancellationToken);
    }

    public async Task<ProblemOrResponse<GetCountriesResponse>> GetCountries(CancellationToken cancellationToken = default)
    {
        RestRequest request = Get("/admin/api/{apiVersion}/countries")
            .UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion);

        return await _restClient.SendRequestAsync<GetCountriesResponse>(request, cancellationToken);
    }

    public async Task<ProblemOrResponse<GetCountryResponse>> GetCountry(Guid countryId,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = Get("/admin/api/{apiVersion}/countries/{countryId}")
            .UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("countryId", countryId);

        return await _restClient.SendRequestAsync<GetCountryResponse>(request, cancellationToken);
    }
}
