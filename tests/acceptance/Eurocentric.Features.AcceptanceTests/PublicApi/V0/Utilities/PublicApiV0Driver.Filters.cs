using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.PublicApi.V0.Filters;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utilities;

public sealed partial class PublicApiV0Driver : IPublicApiV0Driver.IFilters
{
    public async Task<ResponseOrProblem<GetAvailableContestStagesResponse>> GetAvailableContestStages(
        CancellationToken cancellationToken = default)
    {
        RestRequest request = Get("/public/api/{apiVersion}/filters/contest-stages")
            .AddUrlSegment("apiVersion", _apiVersion);

        return await _restClient.SendRequestAsync<GetAvailableContestStagesResponse>(request, cancellationToken);
    }

    public async Task<ResponseOrProblem<GetAvailableVotingMethodsResponse>> GetAvailableVotingMethods(
        CancellationToken cancellationToken = default)
    {
        RestRequest request = Get("/public/api/{apiVersion}/filters/voting-methods")
            .AddUrlSegment("apiVersion", _apiVersion);

        return await _restClient.SendRequestAsync<GetAvailableVotingMethodsResponse>(request, cancellationToken);
    }
}
