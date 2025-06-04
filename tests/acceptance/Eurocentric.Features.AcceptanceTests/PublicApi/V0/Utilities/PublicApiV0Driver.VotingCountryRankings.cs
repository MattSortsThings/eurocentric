using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.PublicApi.V0.VotingCountryRankings;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utilities;

public sealed partial class PublicApiV0Driver : IPublicApiV0Driver.IVotingCountryRankings
{
    public async Task<ProblemOrResponse<GetPointsShareVotingCountryRankingsResponse>> GetPointsShareVotingCountryRankings(
        IReadOnlyDictionary<string, object> queryParams, CancellationToken cancellationToken = default)
    {
        RestRequest request = Get("/public/api/{apiVersion}/voting-country-rankings/points-share")
            .AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameters(queryParams);

        return await _restClient.SendRequestAsync<GetPointsShareVotingCountryRankingsResponse>(request, cancellationToken);
    }
}
