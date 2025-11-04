using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Components.DataAccess.Dapper;
using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Analytics.Rankings.VotingCountries;
using Eurocentric.Domain.Core;

namespace Eurocentric.Components.Gateways;

internal sealed class VotingCountryRankingsGateway(SprocRunner sprocRunner) : IVotingCountryRankingsGateway
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
            .Ensure(RankingsInvariants.LegalCompetingCountrySettings)
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
        >(Sprocs.Dbo.GetVotingCountryPointsAverageRankings, dynamicParameters, cancellationToken);

        return new PointsAverageRankings(first, second);
    }
}
