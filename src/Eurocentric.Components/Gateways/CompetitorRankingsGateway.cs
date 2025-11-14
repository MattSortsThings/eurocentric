using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Components.DataAccess.Dapper;
using Eurocentric.Domain.Analytics.Rankings.Common;
using Eurocentric.Domain.Analytics.Rankings.Competitors;
using Eurocentric.Domain.Core;

namespace Eurocentric.Components.Gateways;

internal sealed class CompetitorRankingsGateway(SingleThenListSprocRunner sprocRunner) : ICompetitorRankingsGateway
{
    public async Task<Result<PointsAverageRankings, IDomainError>> GetPointsAverageRankingsAsync(
        PointsAverageQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return await Result
            .Success<PointsAverageQuery, IDomainError>(query)
            .Ensure(RankingsInvariants.LegalBroadcastFiltering)
            .Ensure(RankingsInvariants.LegalCompetingCountryFiltering)
            .Ensure(RankingsInvariants.LegalPaginationOverrides)
            .Bind(queryParams => RunSprocAsync(queryParams, cancellationToken));
    }

    public async Task<Result<PointsConsensusRankings, IDomainError>> GetPointsConsensusRankingsAsync(
        PointsConsensusQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return await Result
            .Success<PointsConsensusQuery, IDomainError>(query)
            .Ensure(RankingsInvariants.LegalBroadcastFiltering)
            .Ensure(RankingsInvariants.LegalCompetingCountryFiltering)
            .Ensure(RankingsInvariants.LegalPaginationOverrides)
            .Bind(queryParams => RunSprocAsync(queryParams, cancellationToken));
    }

    public async Task<Result<PointsInRangeRankings, IDomainError>> GetPointsInRangeRankingsAsync(
        PointsInRangeQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return await Result
            .Success<PointsInRangeQuery, IDomainError>(query)
            .Ensure(RankingsInvariants.LegalBroadcastFiltering)
            .Ensure(RankingsInvariants.LegalCompetingCountryFiltering)
            .Ensure(RankingsInvariants.LegalPaginationOverrides)
            .Ensure(RankingsInvariants.LegalPointsValueRange)
            .Bind(queryParams => RunSprocAsync(queryParams, cancellationToken));
    }

    public async Task<Result<PointsShareRankings, IDomainError>> GetPointsShareRankingsAsync(
        PointsShareQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return await Result
            .Success<PointsShareQuery, IDomainError>(query)
            .Ensure(RankingsInvariants.LegalBroadcastFiltering)
            .Ensure(RankingsInvariants.LegalCompetingCountryFiltering)
            .Ensure(RankingsInvariants.LegalPaginationOverrides)
            .Bind(queryParams => RunSprocAsync(queryParams, cancellationToken));
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
        >(Sprocs.Dbo.GetCompetitorPointsAverageRankings, dynamicParameters, cancellationToken);

        return new PointsAverageRankings(rankings, metadata);
    }

    private async Task<Result<PointsConsensusRankings, IDomainError>> RunSprocAsync(
        PointsConsensusQuery query,
        CancellationToken cancellationToken
    )
    {
        RankingsDynamicParameters dynamicParameters = RankingsDynamicParameters.From(query);

        (PointsConsensusMetadata metadata, List<PointsConsensusRanking> rankings) = await sprocRunner.ExecuteAsync<
            PointsConsensusMetadata,
            PointsConsensusRanking
        >(Sprocs.Dbo.GetCompetitorPointsConsensusRankings, dynamicParameters, cancellationToken);

        return new PointsConsensusRankings(rankings, metadata);
    }

    private async Task<Result<PointsInRangeRankings, IDomainError>> RunSprocAsync(
        PointsInRangeQuery query,
        CancellationToken cancellationToken
    )
    {
        RankingsDynamicParameters dynamicParameters = RankingsDynamicParameters.From(query);

        (PointsInRangeMetadata metadata, List<PointsInRangeRanking> rankings) = await sprocRunner.ExecuteAsync<
            PointsInRangeMetadata,
            PointsInRangeRanking
        >(Sprocs.Dbo.GetCompetitorPointsInRangeRankings, dynamicParameters, cancellationToken);

        return new PointsInRangeRankings(rankings, metadata);
    }

    private async Task<Result<PointsShareRankings, IDomainError>> RunSprocAsync(
        PointsShareQuery query,
        CancellationToken cancellationToken
    )
    {
        RankingsDynamicParameters dynamicParameters = RankingsDynamicParameters.From(query);

        (PointsShareMetadata metadata, List<PointsShareRanking> rankings) = await sprocRunner.ExecuteAsync<
            PointsShareMetadata,
            PointsShareRanking
        >(Sprocs.Dbo.GetCompetitorPointsShareRankings, dynamicParameters, cancellationToken);

        return new PointsShareRankings(rankings, metadata);
    }
}
