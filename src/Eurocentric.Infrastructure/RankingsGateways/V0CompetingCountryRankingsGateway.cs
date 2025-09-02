using ErrorOr;
using Eurocentric.Domain.V0.Rankings.CompetingCountries;
using Eurocentric.Infrastructure.DataAccess.Common;
using Eurocentric.Infrastructure.DataAccess.Dapper;

namespace Eurocentric.Infrastructure.RankingsGateways;

internal sealed class V0CompetingCountryRankingsGateway(IDbStoredProcedureRunner sprocRunner) : ICompetingCountryRankingsGateway
{
    public async Task<ErrorOr<PointsInRangeRankingsPage>> GetPointsInRangeRankingsPageAsync(PointsInRangeQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await query.ToErrorOr()
            .Then(PrepareForSproc)
            .ThenAsync(async step1 => await ExecuteSprocAsync(step1, cancellationToken))
            .Then(MapToRankingsPage);
    }

    private ErrorOr<PointsInRangeStep1> PrepareForSproc(PointsInRangeQuery clientQuery) => RankingsDynamicParameters.Builder
        .InitializeWithPagination(clientQuery.PageIndex, clientQuery.PageSize, clientQuery.Descending)
        .AddOptionalContestStages(clientQuery.ContestStage)
        .AddOptionalContestYearRange(clientQuery.MinYear, clientQuery.MaxYear)
        .AddOptionalVotingCountry(clientQuery.VotingCountryCode)
        .AddOptionalVotingMethods(clientQuery.VotingMethod)
        .AddPointsValueRange(clientQuery.MinPoints, clientQuery.MaxPoints)
        .Get().Then(dynamicParameters => new PointsInRangeStep1(clientQuery, dynamicParameters));

    private async Task<PointsInRangeStep2> ExecuteSprocAsync(PointsInRangeStep1 step1, CancellationToken cancellationToken)
    {
        (PointsInRangeQuery clientQuery, RankingsDynamicParameters parameters) = step1;

        List<PointsInRangeRanking> rankings = await sprocRunner.ExecuteAsync<PointsInRangeRanking>(
            DbStoredProcedureNames.V0.GetCompetingCountryPointsInRangeRankings,
            parameters,
            cancellationToken);

        return new PointsInRangeStep2(clientQuery, rankings, parameters.GetPaginationOutputs());
    }


    private static PointsInRangeRankingsPage MapToRankingsPage(PointsInRangeStep2 step2)
    {
        (PointsInRangeQuery clientQuery, List<PointsInRangeRanking> rankings, (int totalItems, int totalPages)) = step2;

        PointsInRangeMetadata metadata = new()
        {
            PageIndex = clientQuery.PageIndex,
            PageSize = clientQuery.PageSize,
            Descending = clientQuery.Descending,
            TotalItems = totalItems,
            TotalPages = totalPages,
            MinPoints = clientQuery.MinPoints,
            MaxPoints = clientQuery.MaxPoints,
            MinYear = clientQuery.MinYear,
            MaxYear = clientQuery.MaxYear,
            ContestStage = clientQuery.ContestStage,
            VotingCountryCode = clientQuery.VotingCountryCode,
            VotingMethod = clientQuery.VotingMethod
        };

        return new PointsInRangeRankingsPage(rankings, metadata);
    }

    private sealed record PointsInRangeStep1(PointsInRangeQuery ClientQuery, RankingsDynamicParameters Parameters);

    private sealed record PointsInRangeStep2(
        PointsInRangeQuery ClientQuery,
        List<PointsInRangeRanking> Rankings,
        PaginationOutputs PaginationOutputs);
}
