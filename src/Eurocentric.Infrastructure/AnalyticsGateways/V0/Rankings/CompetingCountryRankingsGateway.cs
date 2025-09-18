using ErrorOr;
using Eurocentric.Domain.V0Analytics.Rankings.Common;
using Eurocentric.Domain.V0Analytics.Rankings.CompetingCountries;
using Eurocentric.Infrastructure.DataAccess.Constants;
using Eurocentric.Infrastructure.DataAccess.Dapper;

namespace Eurocentric.Infrastructure.AnalyticsGateways.V0.Rankings;

internal sealed class CompetingCountryRankingsGateway(IDbSprocRunner sprocRunner) : ICompetingCountryRankingsGateway
{
    public async Task<ErrorOr<PointsInRangeRankingsPage>> GetPointsInRangeRankingsPageAsync(PointsInRangeQuery query,
        CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        return await query.ToErrorOr()
            .FailOnIllegalPagination()
            .FailOnIllegalPointsRange()
            .FailOnIllegalVotingCountryCode()
            .FailOnIllegalYearRange()
            .ThenAsync(pointsInRangeQuery => ExecuteSprocAsync(pointsInRangeQuery, cancellationToken));
    }

    private async Task<PointsInRangeRankingsPage> ExecuteSprocAsync(PointsInRangeQuery query,
        CancellationToken cancellationToken = default)
    {
        RankingsDynamicParameters parameters = BuildDynamicParameters(query);

        PointsInRangeRanking[] rankings = await sprocRunner.ExecuteAsync<PointsInRangeRanking>(
            V0Schema.Sprocs.GetCompetingCountryPointsInRangeRankings,
            parameters,
            cancellationToken);

        (int totalPages, int totalRankings) = parameters.GetRankingsTotals();

        PointsInRangeMetadata metadata = new()
        {
            MinPoints = query.MinPoints,
            MaxPoints = query.MaxPoints,
            MinYear = query.MinYear,
            MaxYear = query.MaxYear,
            ContestStages = query.ContestStages,
            VotingMethod = query.VotingMethod,
            VotingCountryCode = query.VotingCountryCode?.ToUpperInvariant(),
            PageIndex = query.PageIndex,
            PageSize = query.PageSize,
            Descending = query.Descending,
            TotalPages = totalPages,
            TotalRankings = totalRankings
        };

        return new PointsInRangeRankingsPage(rankings, metadata);
    }

    private static RankingsDynamicParameters BuildDynamicParameters(PointsInRangeQuery query)
    {
        RankingsDynamicParameters dynamicParameters = new();

        dynamicParameters.AddPaginationParametersFrom(query);
        dynamicParameters.AddPointsRangeParametersFrom(query);
        dynamicParameters.AddContestStagesParameterFrom(query);
        dynamicParameters.AddOptionalVotingCountryParameterFrom(query);
        dynamicParameters.AddVotingMethodParametersFrom(query);
        dynamicParameters.AddOptionalYearRangeParametersFrom(query);

        return dynamicParameters;
    }
}
