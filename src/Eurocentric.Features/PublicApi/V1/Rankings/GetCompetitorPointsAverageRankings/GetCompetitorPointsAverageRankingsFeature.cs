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

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetitorPointsAverageRankings;

internal static class GetCompetitorPointsAverageRankingsFeature
{
    internal static async Task<IResult> ExecuteAsync([AsParameters] GetCompetitorPointsAverageRankingsRequest queryParams,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(queryParams.ToQuery(), TypedResults.Ok, cancellationToken);

    private static Query ToQuery(this GetCompetitorPointsAverageRankingsRequest queryParams) => new()
    {
        PageIndex = queryParams.PageIndex.GetValueOrDefault(QueryParamDefaults.PageIndex),
        PageSize = queryParams.PageSize.GetValueOrDefault(QueryParamDefaults.PageSize),
        Descending = queryParams.Descending.GetValueOrDefault(QueryParamDefaults.Descending),
        ContestStage = queryParams.ContestStage.GetValueOrDefault(QueryParamDefaults.ContestStage),
        MinYear = queryParams.MinYear,
        MaxYear = queryParams.MaxYear,
        VotingMethod = queryParams.VotingMethod.GetValueOrDefault(QueryParamDefaults.VotingMethod)
    };

    private static CompetitorPointsAverageFilteringMetadata GetFilteringMetadata(this Query query) => new()
    {
        ContestStage = query.ContestStage,
        MinYear = query.MinYear,
        MaxYear = query.MaxYear,
        VotingMethod = query.VotingMethod
    };

    private static async Task<(CompetitorPointsAverageRanking[], PaginationMetadata)>
        ExecuteStoredProcedure(this IDbStoredProcedureRunner procRunner,
            StoredProcedureParams procParams,
            CancellationToken cancellationToken)
    {
        CompetitorPointsAverageRanking[] rankings = await procRunner.ExecuteAsync<CompetitorPointsAverageRanking>(
            DbStoredProcedures.Dbo.GetCompetitorPointsAverageRankings,
            procParams,
            cancellationToken);

        return (rankings, procParams.GetPaginationMetadata());
    }

    internal sealed record Query : IQuery<GetCompetitorPointsAverageRankingsResponse>,
        IContestStageFilteringQuery,
        IPaginatedQuery,
        IVotingMethodFilteringQuery,
        IOptionalYearRangeFilteringQuery
    {
        public QueryableContestStage ContestStage { get; init; }

        public int? MinYear { get; init; }

        public int? MaxYear { get; init; }

        public int PageIndex { get; init; }

        public int PageSize { get; init; }

        public bool Descending { get; init; }

        public QueryableVotingMethod VotingMethod { get; init; }
    }

    [UsedImplicitly]
    internal sealed class QueryHandler(IDbStoredProcedureRunner procRunner)
        : IQueryHandler<Query, GetCompetitorPointsAverageRankingsResponse>
    {
        public async Task<ErrorOr<GetCompetitorPointsAverageRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken)
        {
            CompetitorPointsAverageFilteringMetadata filtering = query.GetFilteringMetadata();

            return await StoredProcedureParams.CreateWithPaginationParamsFrom(query)
                .WithContestStagesParamFrom(query)
                .WithOptionalYearRangeParamsFrom(query)
                .WithVotingMethodParamsFrom(query)
                .ThenAsync(procParams => procRunner.ExecuteStoredProcedure(procParams, cancellationToken))
                .Then(tuple => new GetCompetitorPointsAverageRankingsResponse(tuple.Item1, filtering, tuple.Item2));
        }
    }
}
