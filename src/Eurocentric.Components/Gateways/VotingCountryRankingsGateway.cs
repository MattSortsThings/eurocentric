using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Components.DataAccess.Dapper;
using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Analytics.Rankings.VotingCountries;
using Eurocentric.Domain.Core;

namespace Eurocentric.Components.Gateways;

internal sealed class VotingCountryRankingsGateway(SingleThenListSprocRunner sprocRunner)
    : IVotingCountryRankingsGateway
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
            .Ensure(RankingsInvariants.LegalCompetingCountryFiltering)
            .Bind(pointsAverageQuery => RunSprocAsync(pointsAverageQuery, cancellationToken));
    }

    private async Task<Result<PointsAverageRankings, IDomainError>> RunSprocAsync(
        PointsAverageQuery query,
        CancellationToken cancellationToken
    )
    {
        RankingsDynamicParameters dynamicParameters = RankingsDynamicParameters.From(query);

        (PointsAverageMetadata metadata, List<PointsAverageRanking> rankings) = await sprocRunner.ExecuteAsync<
            PointsAverageMetadata,
            PointsAverageRanking
        >(Sprocs.Dbo.GetVotingCountryPointsAverageRankings, dynamicParameters, cancellationToken);

        return new PointsAverageRankings(rankings, metadata);
    }
}
