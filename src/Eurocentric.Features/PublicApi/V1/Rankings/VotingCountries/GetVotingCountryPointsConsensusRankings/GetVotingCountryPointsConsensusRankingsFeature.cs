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

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsConsensusRankings;

internal static class GetVotingCountryPointsConsensusRankingsFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [AsParameters] GetVotingCountryPointsConsensusRankingsRequest queryParams,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(queryParams.ToQuery(), TypedResults.Ok, cancellationToken);

    private static Query ToQuery(this GetVotingCountryPointsConsensusRankingsRequest queryParams) => new()
    {
        PageIndex = queryParams.PageIndex.GetValueOrDefault(QueryParamDefaults.PageIndex),
        PageSize = queryParams.PageSize.GetValueOrDefault(QueryParamDefaults.PageSize),
        Descending = queryParams.Descending.GetValueOrDefault(QueryParamDefaults.Descending),
        ContestStage = queryParams.ContestStage.GetValueOrDefault(QueryParamDefaults.ContestStage),
        MinYear = queryParams.MinYear,
        MaxYear = queryParams.MaxYear,
        CompetingCountryCode = queryParams.CompetingCountryCode
    };

    private static VotingCountryPointsConsensusQueryMetadata ToQueryMetadata(this Query query) => new()
    {
        ContestStage = query.ContestStage,
        MinYear = query.MinYear,
        MaxYear = query.MaxYear,
        CompetingCountryCode = query.CompetingCountryCode
    };

    private static async Task<(VotingCountryPointsConsensusRanking[] rankings, PaginationMetadata pagination)> RunAsync(
        this IDbStoredProcedureRunner procRunner,
        StoredProcedureParams procParams,
        CancellationToken cancellationToken)
    {
        VotingCountryPointsConsensusRanking[] rankings =
            await procRunner.ExecuteAsync<VotingCountryPointsConsensusRanking>(
                DbStoredProcedures.Dbo.GetVotingCountryPointsConsensusRankings,
                procParams,
                cancellationToken);

        return (rankings, procParams.GetPaginationMetadata());
    }

    internal sealed record Query : IQuery<GetVotingCountryPointsConsensusRankingsResponse>,
        IContestStageFilteringQuery,
        IPaginatedQuery,
        IOptionalCompetingCountryFilteringQuery,
        IOptionalYearRangeFilteringQuery
    {
        public QueryableContestStage ContestStage { get; init; }

        public string? CompetingCountryCode { get; init; }

        public int? MinYear { get; init; }

        public int? MaxYear { get; init; }

        public int PageIndex { get; init; }

        public int PageSize { get; init; }

        public bool Descending { get; init; }
    }

    [UsedImplicitly]
    internal sealed class QueryHandler(IDbStoredProcedureRunner procRunner)
        : IQueryHandler<Query, GetVotingCountryPointsConsensusRankingsResponse>
    {
        public async Task<ErrorOr<GetVotingCountryPointsConsensusRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken) => await StoredProcedureParams.CreateWithPaginationParamsFrom(query)
            .WithContestStagesParamFrom(query)
            .WithOptionalCompetingCountryCodeParamFrom(query)
            .WithOptionalYearRangeParamsFrom(query)
            .ThenAsync(procParams => procRunner.RunAsync(procParams, cancellationToken))
            .Then(tuple => new GetVotingCountryPointsConsensusRankingsResponse(tuple.rankings,
                query.ToQueryMetadata(),
                tuple.pagination));
    }
}
