using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Countries;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

public sealed partial class AdminApiV1Driver
{
    public async Task<Country[]> CreateMultipleCountriesAsync(IEnumerable<string> countryCodes,
        CancellationToken cancellationToken = default)
    {
        Task<ResponseOrProblem<CreateCountryResponse>>[] tasks = countryCodes
            .Select(code => new CreateCountryRequest { CountryCode = code, CountryName = code.ToCountryName() })
            .Select(request => CreateCountryAsync(request, cancellationToken))
            .ToArray();

        ResponseOrProblem<CreateCountryResponse>[] results = await Task.WhenAll(tasks);

        return results.Select(r => r.AsT0.Data!.Country).ToArray();
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

    public async Task<ResponseOrProblem<GetCountriesResponse>> GetAllCountriesAsync(
        CancellationToken cancellationToken = default)
    {
        RestRequest request = new("/admin/api/v{apiVersion}/countries");

        request.UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion);

        return await _client.SendRequestAsync<GetCountriesResponse>(request, cancellationToken);
    }
}
