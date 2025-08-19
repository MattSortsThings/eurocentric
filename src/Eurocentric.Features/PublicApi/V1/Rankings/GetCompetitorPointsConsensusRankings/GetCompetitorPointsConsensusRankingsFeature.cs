using ErrorOr;
using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.DataAccess;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.Common;
using Eurocentric.Infrastructure.DataAccess.Dapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetitorPointsConsensusRankings;

internal static class GetCompetitorPointsConsensusRankingsFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [AsParameters] GetCompetitorPointsConsensusRankingsRequest queryParams,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(queryParams.ToQuery(), TypedResults.Ok, cancellationToken);

    private static Query ToQuery(this GetCompetitorPointsConsensusRankingsRequest queryParams) => new()
    {
        PageIndex = queryParams.PageIndex.GetValueOrDefault(QueryParamDefaults.PageIndex),
        PageSize = queryParams.PageSize.GetValueOrDefault(QueryParamDefaults.PageSize),
        Descending = queryParams.Descending.GetValueOrDefault(QueryParamDefaults.Descending),
        ContestStage = queryParams.ContestStage.GetValueOrDefault(QueryParamDefaults.ContestStage),
        MinYear = queryParams.MinYear,
        MaxYear = queryParams.MaxYear
    };

    private static CompetitorPointsConsensusFilteringMetadata GetFilteringMetadata(this Query query) => new()
    {
        ContestStage = query.ContestStage, MinYear = query.MinYear, MaxYear = query.MaxYear
    };

    private static async Task<(CompetitorPointsConsensusRanking[], PaginationMetadata)>
        ExecuteStoredProcedure(this IDbStoredProcedureRunner procRunner,
            StoredProcedureParams procParams,
            CancellationToken cancellationToken)
    {
        CompetitorPointsConsensusRanking[] rankings =
            await procRunner.ExecuteAsync<CompetitorPointsConsensusRanking>(
                DbStoredProcedures.Dbo.GetCompetitorPointsConsensusRankings,
                procParams,
                cancellationToken);

        return (rankings, procParams.GetPaginationMetadata());
    }

    internal sealed record Query : IQuery<GetCompetitorPointsConsensusRankingsResponse>,
        IContestStageFilteringQuery,
        IPaginatedQuery,
        IOptionalYearRangeFilteringQuery
    {
        public QueryableContestStage ContestStage { get; init; }

        public int? MinYear { get; init; }

        public int? MaxYear { get; init; }

        public int PageIndex { get; init; }

        public int PageSize { get; init; }

        public bool Descending { get; init; }
    }

    [UsedImplicitly]
    internal sealed class QueryHandler(IDbStoredProcedureRunner procRunner)
        : IQueryHandler<Query, GetCompetitorPointsConsensusRankingsResponse>
    {
        public async Task<ErrorOr<GetCompetitorPointsConsensusRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken)
        {
            CompetitorPointsConsensusFilteringMetadata filtering = query.GetFilteringMetadata();

            return await StoredProcedureParams.CreateWithPaginationParamsFrom(query)
                .WithContestStagesParamFrom(query)
                .WithOptionalYearRangeParamsFrom(query)
                .ThenAsync(procParams => procRunner.ExecuteStoredProcedure(procParams, cancellationToken))
                .Then(tuple => new GetCompetitorPointsConsensusRankingsResponse(tuple.Item1, filtering, tuple.Item2));
        }
    }
}
