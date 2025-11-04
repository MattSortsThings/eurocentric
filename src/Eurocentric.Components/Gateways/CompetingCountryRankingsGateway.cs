using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Components.DataAccess.Dapper;
using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Analytics.Rankings.CompetingCountries;
using Eurocentric.Domain.Core;

namespace Eurocentric.Components.Gateways;

internal sealed class CompetingCountryRankingsGateway(SprocRunner sprocRunner) : ICompetingCountryRankingsGateway
{
    public async Task<Result<PointsAverageRankings, IDomainError>> GetPointsAverageRankingsAsync(
        PointsAverageQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return await Result
            .Success<PointsAverageQuery, IDomainError>(query)
            .Ensure(RankingsInvariants.LegalBroadcastFiltering)
            .Ensure(RankingsInvariants.LegalPaginationSettings)
            .Ensure(RankingsInvariants.LegalVotingCountryFiltering)
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
        >(Sprocs.Dbo.GetCompetingCountryPointsAverageRankings, dynamicParameters, cancellationToken);

        return new PointsAverageRankings(first, second);
    }
}
