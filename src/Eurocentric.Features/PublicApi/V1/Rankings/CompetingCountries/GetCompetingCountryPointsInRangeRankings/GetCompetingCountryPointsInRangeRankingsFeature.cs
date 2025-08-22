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

namespace Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsInRangeRankings;

internal static class GetCompetingCountryPointsInRangeRankingsFeature
{
    internal static async Task<IResult> ExecuteAsync([AsParameters] GetCompetingCountryPointsInRangeRankingsRequest queryParams,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(queryParams.ToQuery(), TypedResults.Ok, cancellationToken);

    private static Query ToQuery(this GetCompetingCountryPointsInRangeRankingsRequest queryParams) => new()
    {
        PageIndex = queryParams.PageIndex.GetValueOrDefault(QueryParamDefaults.PageIndex),
        PageSize = queryParams.PageSize.GetValueOrDefault(QueryParamDefaults.PageSize),
        Descending = queryParams.Descending.GetValueOrDefault(QueryParamDefaults.Descending),
        ContestStage = queryParams.ContestStage.GetValueOrDefault(QueryParamDefaults.ContestStage),
        MinPoints = queryParams.MinPoints,
        MaxPoints = queryParams.MaxPoints,
        MinYear = queryParams.MinYear,
        MaxYear = queryParams.MaxYear,
        VotingCountryCode = queryParams.VotingCountryCode,
        VotingMethod = queryParams.VotingMethod.GetValueOrDefault(QueryParamDefaults.VotingMethod)
    };

    private static CompetingCountryPointsInRangeQueryMetadata ToQueryMetadata(this Query query) => new()
    {
        ContestStage = query.ContestStage,
        MinPoints = query.MinPoints,
        MaxPoints = query.MaxPoints,
        MinYear = query.MinYear,
        MaxYear = query.MaxYear,
        VotingCountryCode = query.VotingCountryCode,
        VotingMethod = query.VotingMethod
    };

    private static async Task<(CompetingCountryPointsInRangeRanking[] rankings, PaginationMetadata pagination)> RunAsync(
        this IDbStoredProcedureRunner procRunner,
        StoredProcedureParams procParams,
        CancellationToken cancellationToken)
    {
        CompetingCountryPointsInRangeRanking[] rankings = await procRunner.ExecuteAsync<CompetingCountryPointsInRangeRanking>(
            DbStoredProcedures.Dbo.GetCompetingCountryPointsInRangeRankings,
            procParams,
            cancellationToken);

        return (rankings, procParams.GetPaginationMetadata());
    }

    internal sealed record Query : IQuery<GetCompetingCountryPointsInRangeRankingsResponse>,
        IContestStageFilteringQuery,
        IPaginatedQuery,
        IPointsRangeFilteringQuery,
        IOptionalVotingCountryFilteringQuery,
        IVotingMethodFilteringQuery,
        IOptionalYearRangeFilteringQuery
    {
        public QueryableContestStage ContestStage { get; init; }

        public string? VotingCountryCode { get; init; }

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
        : IQueryHandler<Query, GetCompetingCountryPointsInRangeRankingsResponse>
    {
        public async Task<ErrorOr<GetCompetingCountryPointsInRangeRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken) => await StoredProcedureParams.CreateWithPaginationParamsFrom(query)
            .WithContestStagesParamFrom(query)
            .WithOptionalVotingCountryCodeParamFrom(query)
            .WithOptionalYearRangeParamsFrom(query)
            .WithPointsRangeParamsFrom(query)
            .WithVotingMethodParamsFrom(query)
            .ThenAsync(procParams => procRunner.RunAsync(procParams, cancellationToken))
            .Then(tuple => new GetCompetingCountryPointsInRangeRankingsResponse(tuple.rankings,
                query.ToQueryMetadata(),
                tuple.pagination));
    }
}
