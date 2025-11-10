using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Components.DataAccess.Dapper;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.V0.Queries.Rankings.Common;
using Eurocentric.Domain.V0.Queries.Rankings.CompetingCountries;

namespace Eurocentric.Components.Gateways.V0;

internal sealed class CompetingCountryRankingsGateway(V0SprocRunner sprocRunner) : ICompetingCountryRankingsGateway
{
    public async Task<Result<PointsAverageRankings, IDomainError>> GetPointsAverageRankingsAsync(
        PointsAverageQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return await Result
            .Success<PointsAverageQuery, IDomainError>(query)
            .Ensure(RankingsQueryRules.LegalBroadcastFiltering)
            .Ensure(RankingsQueryRules.LegalPaginationSettings)
            .Ensure(RankingsQueryRules.LegalVotingCountryFiltering)
            .Bind(pointsAverageQuery => RunSprocAsync(pointsAverageQuery, cancellationToken));
    }

    private async Task<Result<PointsAverageRankings, IDomainError>> RunSprocAsync(
        PointsAverageQuery query,
        CancellationToken cancellationToken
    )
    {
        RankingsDynamicParameters dynamicParameters = RankingsDynamicParameters.From(query);

        (List<PointsAverageRanking> first, PointsAverageMetadata second) = await sprocRunner.ExecuteMultipleAsync<
            PointsAverageRanking,
            PointsAverageMetadata
        >(Sprocs.V0.GetCompetingCountryPointsAverageRankings, dynamicParameters, cancellationToken);

        return new PointsAverageRankings(first, second);
    }
}
