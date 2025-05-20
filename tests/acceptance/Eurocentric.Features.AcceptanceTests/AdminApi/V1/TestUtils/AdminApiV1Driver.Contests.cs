using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Contests;
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

    public async Task<ResponseOrProblem<CreateContestResponse>> CreateContestAsync(CreateContestRequest requestBody,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = new("/admin/api/v{apiVersion}/contests", Method.Post);

        request.UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddJsonBody(requestBody);

        return await _client.SendRequestAsync<CreateContestResponse>(request, cancellationToken);
    }

    public async Task<ResponseOrProblem<GetContestResponse>> GetContestAsync(Guid contestId,
        CancellationToken cancellationToken = default)
    {
        RestRequest request = new("/admin/api/v{apiVersion}/contests/{contestId}");

        request.UseSecretApiKey()
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddUrlSegment("contestId", contestId);

        return await _client.SendRequestAsync<GetContestResponse>(request, cancellationToken);
    }
}
