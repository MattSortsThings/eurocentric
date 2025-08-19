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

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetitorPointsInRangeRankings;

internal static class GetCompetitorPointsInRangeRankingsFeature
{
    internal static async Task<IResult> ExecuteAsync([AsParameters] GetCompetitorPointsInRangeRankingsRequest queryParams,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(queryParams.ToQuery(), TypedResults.Ok, cancellationToken);

    private static Query ToQuery(this GetCompetitorPointsInRangeRankingsRequest queryParams) => new()
    {
        PageIndex = queryParams.PageIndex.GetValueOrDefault(QueryParamDefaults.PageIndex),
        PageSize = queryParams.PageSize.GetValueOrDefault(QueryParamDefaults.PageSize),
        Descending = queryParams.Descending.GetValueOrDefault(QueryParamDefaults.Descending),
        ContestStage = queryParams.ContestStage.GetValueOrDefault(QueryParamDefaults.ContestStage),
        MinPoints = queryParams.MinPoints,
        MaxPoints = queryParams.MaxPoints,
        MinYear = queryParams.MinYear,
        MaxYear = queryParams.MaxYear,
        VotingMethod = queryParams.VotingMethod.GetValueOrDefault(QueryParamDefaults.VotingMethod)
    };

    private static CompetitorPointsInRangeFilteringMetadata GetFilteringMetadata(this Query query) => new()
    {
        ContestStage = query.ContestStage,
        MinPoints = query.MinPoints,
        MaxPoints = query.MaxPoints,
        MinYear = query.MinYear,
        MaxYear = query.MaxYear,
        VotingMethod = query.VotingMethod
    };

    private static async Task<(CompetitorPointsInRangeRanking[], PaginationMetadata)>
        ExecuteStoredProcedure(this IDbStoredProcedureRunner procRunner,
            StoredProcedureParams procParams,
            CancellationToken cancellationToken)
    {
        CompetitorPointsInRangeRanking[] rankings = await procRunner.ExecuteAsync<CompetitorPointsInRangeRanking>(
            DbStoredProcedures.Dbo.GetCompetitorPointsInRangeRankings,
            procParams,
            cancellationToken);

        return (rankings, procParams.GetPaginationMetadata());
    }

    internal sealed record Query : IQuery<GetCompetitorPointsInRangeRankingsResponse>,
        IContestStageFilteringQuery,
        IPaginatedQuery,
        IPointsRangeFilteringQuery,
        IVotingMethodFilteringQuery,
        IOptionalYearRangeFilteringQuery
    {
        public QueryableContestStage ContestStage { get; init; }

        public int? MinYear { get; init; }

        public int? MaxYear { get; init; }

        public int PageIndex { get; init; }

        public int PageSize { get; init; }

        public bool Descending { get; init; }

        public int MinPoints { get; init; }

        public int MaxPoints { get; init; }

        public QueryableVotingMethod VotingMethod { get; init; }
    }

    [UsedImplicitly]
    internal sealed class QueryHandler(IDbStoredProcedureRunner procRunner)
        : IQueryHandler<Query, GetCompetitorPointsInRangeRankingsResponse>
    {
        public async Task<ErrorOr<GetCompetitorPointsInRangeRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken)
        {
            CompetitorPointsInRangeFilteringMetadata filtering = query.GetFilteringMetadata();

            return await StoredProcedureParams.CreateWithPaginationParamsFrom(query)
                .WithContestStagesParamFrom(query)
                .WithOptionalYearRangeParamsFrom(query)
                .WithPointsRangeParamsFrom(query)
                .WithVotingMethodParamsFrom(query)
                .ThenAsync(procParams => procRunner.ExecuteStoredProcedure(procParams, cancellationToken))
                .Then(tuple => new GetCompetitorPointsInRangeRankingsResponse(tuple.Item1, filtering, tuple.Item2));
        }
    }
}
