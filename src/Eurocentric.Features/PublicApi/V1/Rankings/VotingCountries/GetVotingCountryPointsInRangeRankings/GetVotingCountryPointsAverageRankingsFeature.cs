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

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsInRangeRankings;

internal static class GetVotingCountryPointsInRangeRankingsFeature
{
    internal static async Task<IResult> ExecuteAsync([AsParameters] GetVotingCountryPointsInRangeRankingsRequest queryParams,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(queryParams.ToQuery(), TypedResults.Ok, cancellationToken);

    private static Query ToQuery(this GetVotingCountryPointsInRangeRankingsRequest queryParams) => new()
    {
        CompetingCountryCode = queryParams.CompetingCountryCode,
        MinPoints = queryParams.MinPoints,
        MaxPoints = queryParams.MaxPoints,
        PageIndex = queryParams.PageIndex.GetValueOrDefault(QueryParamDefaults.PageIndex),
        PageSize = queryParams.PageSize.GetValueOrDefault(QueryParamDefaults.PageSize),
        Descending = queryParams.Descending.GetValueOrDefault(QueryParamDefaults.Descending),
        ContestStage = queryParams.ContestStage.GetValueOrDefault(QueryParamDefaults.ContestStage),
        MinYear = queryParams.MinYear,
        MaxYear = queryParams.MaxYear,
        VotingMethod = queryParams.VotingMethod.GetValueOrDefault(QueryParamDefaults.VotingMethod)
    };

    private static VotingCountryPointsInRangeQueryMetadata ToQueryMetadata(this Query query) => new()
    {
        CompetingCountryCode = query.CompetingCountryCode,
        MinPoints = query.MinPoints,
        MaxPoints = query.MaxPoints,
        ContestStage = query.ContestStage,
        MinYear = query.MinYear,
        MaxYear = query.MaxYear,
        VotingMethod = query.VotingMethod
    };

    private static async Task<(VotingCountryPointsInRangeRanking[] rankings, PaginationMetadata pagination)> RunAsync(
        this IDbStoredProcedureRunner procRunner,
        StoredProcedureParams procParams,
        CancellationToken cancellationToken)
    {
        VotingCountryPointsInRangeRanking[] rankings = await procRunner.ExecuteAsync<VotingCountryPointsInRangeRanking>(
            DbStoredProcedures.Dbo.GetVotingCountryPointsInRangeRankings,
            procParams,
            cancellationToken);

        return (rankings, procParams.GetPaginationMetadata());
    }

    internal sealed record Query : IQuery<GetVotingCountryPointsInRangeRankingsResponse>,
        ICompetingCountryFilteringQuery,
        IPointsRangeFilteringQuery,
        IContestStageFilteringQuery,
        IPaginatedQuery,
        IVotingMethodFilteringQuery,
        IOptionalYearRangeFilteringQuery
    {
        public required string CompetingCountryCode { get; init; }

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
        : IQueryHandler<Query, GetVotingCountryPointsInRangeRankingsResponse>
    {
        public async Task<ErrorOr<GetVotingCountryPointsInRangeRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken) => await StoredProcedureParams.CreateWithPaginationParamsFrom(query)
            .WithCompetingCountryCodeParamFrom(query)
            .WithPointsRangeParamsFrom(query)
            .WithContestStagesParamFrom(query)
            .WithOptionalYearRangeParamsFrom(query)
            .WithVotingMethodParamsFrom(query)
            .ThenAsync(procParams => procRunner.RunAsync(procParams, cancellationToken))
            .Then(tuple => new GetVotingCountryPointsInRangeRankingsResponse(tuple.rankings,
                query.ToQueryMetadata(),
                tuple.pagination));
    }
}
