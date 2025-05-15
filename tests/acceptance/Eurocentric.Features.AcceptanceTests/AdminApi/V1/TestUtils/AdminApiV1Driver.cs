using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Countries;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

public sealed class AdminApiV1Driver
{
    private readonly string _apiVersion;
    private readonly ITestClient _client;

    private AdminApiV1Driver(ITestClient client, string apiVersion)
    {
        _client = client;
        _apiVersion = apiVersion;
    }

    public async Task<ResponseOrProblem<CreateCountryResponse>> CreateCountryAsync(CreateCountryRequest requestBody,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = new("/admin/api/v{apiVersion}/countries", Method.Post);

        request.UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddJsonBody(requestBody);

        return await _client.SendRequestAsync<CreateCountryResponse>(request, cancellationToken);
    }

    public async Task<ResponseOrProblem<GetCountryResponse>> GetCountryAsync(Guid countryId,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = new("/admin/api/v{apiVersion}/countries/{countryId}");

        request.UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("countryId", countryId);

        return await _client.SendRequestAsync<GetCountryResponse>(request, cancellationToken);
    }

    public static AdminApiV1Driver Create(ITestClient client, int majorVersion, int minorVersion)
    {
        string routePrefix = $"{majorVersion}.{minorVersion}";

        return new AdminApiV1Driver(client, routePrefix);
    }

    public async Task<ResponseOrProblem<GetCountriesResponse>> GetAllCountriesAsync(
        CancellationToken cancellationToken = default)
    {
        RestRequest request = new("/admin/api/v{apiVersion}/countries");

        request.UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion);

        return await _client.SendRequestAsync<GetCountriesResponse>(request, cancellationToken);
    }
}
