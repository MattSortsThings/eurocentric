using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.PublicApi.V0.Filters;
using Eurocentric.Features.PublicApi.V0.VotingCountryRankings;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;

public sealed class PublicApiV0Driver
{
    private readonly string _apiVersion;
    private readonly ITestClient _client;

    private PublicApiV0Driver(ITestClient client, string apiVersion)
    {
        _client = client;
        _apiVersion = apiVersion;
    }

    public static PublicApiV0Driver Create(ITestClient client, int majorVersion, int minorVersion)
    {
        string routePrefix = $"{majorVersion}.{minorVersion}";

        return new PublicApiV0Driver(client, routePrefix);
    }

    public async Task<ResponseOrProblem<GetAvailableVotingMethodsResponse>> GetAvailableVotingMethodsAsync(
        CancellationToken cancellationToken)
    {
        RestRequest request = new("public/api/v{apiVersion}/filters/voting-methods");

        request.AddUrlSegment("apiVersion", _apiVersion)
            .UseDemoApiKey();

        return await _client.SendRequestAsync<GetAvailableVotingMethodsResponse>(request, cancellationToken);
    }

    public async Task<ResponseOrProblem<GetVotingCountryPointsShareRankingsResponse>> GetPointsShareVotingCountryRankingsAsync(
        IReadOnlyDictionary<string, string> queryParams, CancellationToken cancellationToken)
    {
        RestRequest request = new("public/api/v{apiVersion}/voting-country-rankings/points-share");

        request.AddUrlSegment("apiVersion", _apiVersion)
            .UseDemoApiKey();

        foreach ((string key, string value) in queryParams)
        {
            request.AddQueryParameter(key, value);
        }

        return await _client.SendRequestAsync<GetVotingCountryPointsShareRankingsResponse>(request, cancellationToken);
    }
}
