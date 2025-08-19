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

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsConsensusRankings;

internal static class GetCompetingCountryPointsConsensusRankingsFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [AsParameters] GetCompetingCountryPointsConsensusRankingsRequest queryParams,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(queryParams.ToQuery(), TypedResults.Ok, cancellationToken);

    private static Query ToQuery(this GetCompetingCountryPointsConsensusRankingsRequest queryParams) => new()
    {
        PageIndex = queryParams.PageIndex.GetValueOrDefault(QueryParamDefaults.PageIndex),
        PageSize = queryParams.PageSize.GetValueOrDefault(QueryParamDefaults.PageSize),
        Descending = queryParams.Descending.GetValueOrDefault(QueryParamDefaults.Descending),
        ContestStage = queryParams.ContestStage.GetValueOrDefault(QueryParamDefaults.ContestStage),
        MinYear = queryParams.MinYear,
        MaxYear = queryParams.MaxYear,
        VotingCountryCode = queryParams.VotingCountryCode
    };

    private static CompetingCountryPointsConsensusFilteringMetadata GetFilteringMetadata(this Query query) => new()
    {
        ContestStage = query.ContestStage,
        MinYear = query.MinYear,
        MaxYear = query.MaxYear,
        VotingCountryCode = query.VotingCountryCode
    };

    private static async Task<(CompetingCountryPointsConsensusRanking[], PaginationMetadata)>
        ExecuteStoredProcedure(this IDbStoredProcedureRunner procRunner,
            StoredProcedureParams procParams,
            CancellationToken cancellationToken)
    {
        CompetingCountryPointsConsensusRanking[] rankings =
            await procRunner.ExecuteAsync<CompetingCountryPointsConsensusRanking>(
                DbStoredProcedures.Dbo.GetCompetingCountryPointsConsensusRankings,
                procParams,
                cancellationToken);

        return (rankings, procParams.GetPaginationMetadata());
    }

    internal sealed record Query : IQuery<GetCompetingCountryPointsConsensusRankingsResponse>,
        IContestStageFilteringQuery,
        IPaginatedQuery,
        IOptionalVotingCountryFilteringQuery,
        IOptionalYearRangeFilteringQuery
    {
        public QueryableContestStage ContestStage { get; init; }

        public string? VotingCountryCode { get; init; }

        public int? MinYear { get; init; }

        public int? MaxYear { get; init; }

        public int PageIndex { get; init; }

        public int PageSize { get; init; }

        public bool Descending { get; init; }
    }

    [UsedImplicitly]
    internal sealed class QueryHandler(IDbStoredProcedureRunner procRunner)
        : IQueryHandler<Query, GetCompetingCountryPointsConsensusRankingsResponse>
    {
        public async Task<ErrorOr<GetCompetingCountryPointsConsensusRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken)
        {
            CompetingCountryPointsConsensusFilteringMetadata filtering = query.GetFilteringMetadata();

            return await StoredProcedureParams.CreateWithPaginationParamsFrom(query)
                .WithContestStagesParamFrom(query)
                .WithOptionalVotingCountryCodeParamFrom(query)
                .WithOptionalYearRangeParamsFrom(query)
                .ThenAsync(procParams => procRunner.ExecuteStoredProcedure(procParams, cancellationToken))
                .Then(tuple => new GetCompetingCountryPointsConsensusRankingsResponse(tuple.Item1, filtering, tuple.Item2));
        }
    }
}
