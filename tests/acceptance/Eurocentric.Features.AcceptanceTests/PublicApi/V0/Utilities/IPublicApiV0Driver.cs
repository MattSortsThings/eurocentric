using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.PublicApi.V0.Filters;
using Eurocentric.Features.PublicApi.V0.VotingCountryRankings;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utilities;

public interface IPublicApiV0Driver
{
    public IFilters Filters { get; }

    public IVotingCountryRankings VotingCountryRankings { get; }

    public interface IFilters
    {
        public Task<ResponseOrProblem<GetAvailableContestStagesResponse>> GetAvailableContestStages(
            CancellationToken cancellationToken = default);

        public Task<ResponseOrProblem<GetAvailableVotingMethodsResponse>> GetAvailableVotingMethods(
            CancellationToken cancellationToken = default);
    }

    public interface IVotingCountryRankings
    {
        public Task<ResponseOrProblem<GetPointsShareVotingCountryRankingsResponse>> GetPointsShareVotingCountryRankings(
            IReadOnlyDictionary<string, object> queryParams, CancellationToken cancellationToken = default);
    }
}
