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

namespace Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsAverageRankings;

internal static class GetCompetingCountryPointsAverageRankingsFeature
{
    internal static async Task<IResult> ExecuteAsync([AsParameters] GetCompetingCountryPointsAverageRankingsRequest queryParams,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(queryParams.ToQuery(), TypedResults.Ok, cancellationToken);

    private static Query ToQuery(this GetCompetingCountryPointsAverageRankingsRequest queryParams) => new()
    {
        PageIndex = queryParams.PageIndex.GetValueOrDefault(QueryParamDefaults.PageIndex),
        PageSize = queryParams.PageSize.GetValueOrDefault(QueryParamDefaults.PageSize),
        Descending = queryParams.Descending.GetValueOrDefault(QueryParamDefaults.Descending),
        ContestStage = queryParams.ContestStage.GetValueOrDefault(QueryParamDefaults.ContestStage),
        MinYear = queryParams.MinYear,
        MaxYear = queryParams.MaxYear,
        VotingCountryCode = queryParams.VotingCountryCode,
        VotingMethod = queryParams.VotingMethod.GetValueOrDefault(QueryParamDefaults.VotingMethod)
    };

    private static CompetingCountryPointsAverageQueryMetadata ToQueryMetadata(this Query query) => new()
    {
        ContestStage = query.ContestStage,
        MinYear = query.MinYear,
        MaxYear = query.MaxYear,
        VotingCountryCode = query.VotingCountryCode,
        VotingMethod = query.VotingMethod
    };

    private static async Task<(CompetingCountryPointsAverageRanking[] rankings, PaginationMetadata pagination)> RunAsync(
        this IDbStoredProcedureRunner procRunner,
        StoredProcedureParams procParams,
        CancellationToken cancellationToken)
    {
        CompetingCountryPointsAverageRanking[] rankings = await procRunner.ExecuteAsync<CompetingCountryPointsAverageRanking>(
            DbStoredProcedures.Dbo.GetCompetingCountryPointsAverageRankings,
            procParams,
            cancellationToken);

        return (rankings, procParams.GetPaginationMetadata());
    }

    internal sealed record Query : IQuery<GetCompetingCountryPointsAverageRankingsResponse>,
        IContestStageFilteringQuery,
        IPaginatedQuery,
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

        public QueryableVotingMethod VotingMethod { get; init; }
    }

    [UsedImplicitly]
    internal sealed class QueryHandler(IDbStoredProcedureRunner procRunner)
        : IQueryHandler<Query, GetCompetingCountryPointsAverageRankingsResponse>
    {
        public async Task<ErrorOr<GetCompetingCountryPointsAverageRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken) => await StoredProcedureParams.CreateWithPaginationParamsFrom(query)
            .WithContestStagesParamFrom(query)
            .WithOptionalVotingCountryCodeParamFrom(query)
            .WithOptionalYearRangeParamsFrom(query)
            .WithVotingMethodParamsFrom(query)
            .ThenAsync(procParams => procRunner.RunAsync(procParams, cancellationToken))
            .Then(tuple => new GetCompetingCountryPointsAverageRankingsResponse(tuple.rankings,
                query.ToQueryMetadata(),
                tuple.pagination));
    }
}
