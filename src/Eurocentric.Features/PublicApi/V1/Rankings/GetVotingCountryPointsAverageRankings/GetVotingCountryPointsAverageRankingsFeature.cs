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

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetVotingCountryPointsAverageRankings;

internal static class GetVotingCountryPointsAverageRankingsFeature
{
    internal static async Task<IResult> ExecuteAsync([AsParameters] GetVotingCountryPointsAverageRankingsRequest queryParams,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(queryParams.ToQuery(), TypedResults.Ok, cancellationToken);

    private static Query ToQuery(this GetVotingCountryPointsAverageRankingsRequest queryParams) => new()
    {
        CompetingCountryCode = queryParams.CompetingCountryCode,
        PageIndex = queryParams.PageIndex.GetValueOrDefault(QueryParamDefaults.PageIndex),
        PageSize = queryParams.PageSize.GetValueOrDefault(QueryParamDefaults.PageSize),
        Descending = queryParams.Descending.GetValueOrDefault(QueryParamDefaults.Descending),
        ContestStage = queryParams.ContestStage.GetValueOrDefault(QueryParamDefaults.ContestStage),
        MinYear = queryParams.MinYear,
        MaxYear = queryParams.MaxYear,
        VotingMethod = queryParams.VotingMethod.GetValueOrDefault(QueryParamDefaults.VotingMethod)
    };

    private static VotingCountryPointsAverageFilteringMetadata GetFilteringMetadata(this Query query) => new()
    {
        CompetingCountryCode = query.CompetingCountryCode,
        ContestStage = query.ContestStage,
        MinYear = query.MinYear,
        MaxYear = query.MaxYear,
        VotingMethod = query.VotingMethod
    };

    private static async Task<(VotingCountryPointsAverageRanking[], PaginationMetadata)>
        ExecuteStoredProcedure(this IDbStoredProcedureRunner procRunner,
            StoredProcedureParams procParams,
            CancellationToken cancellationToken)
    {
        VotingCountryPointsAverageRanking[] rankings = await procRunner.ExecuteAsync<VotingCountryPointsAverageRanking>(
            DbStoredProcedures.Dbo.GetVotingCountryPointsAverageRankings,
            procParams,
            cancellationToken);

        return (rankings, procParams.GetPaginationMetadata());
    }

    internal sealed record Query : IQuery<GetVotingCountryPointsAverageRankingsResponse>,
        ICompetingCountryFilteringQuery,
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

        public QueryableVotingMethod VotingMethod { get; init; }
    }

    [UsedImplicitly]
    internal sealed class QueryHandler(IDbStoredProcedureRunner procRunner)
        : IQueryHandler<Query, GetVotingCountryPointsAverageRankingsResponse>
    {
        public async Task<ErrorOr<GetVotingCountryPointsAverageRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken)
        {
            VotingCountryPointsAverageFilteringMetadata filtering = query.GetFilteringMetadata();

            return await StoredProcedureParams.CreateWithPaginationParamsFrom(query)
                .WithCompetingCountryCodeParamFrom(query)
                .WithContestStagesParamFrom(query)
                .WithOptionalYearRangeParamsFrom(query)
                .WithVotingMethodParamsFrom(query)
                .ThenAsync(procParams => procRunner.ExecuteStoredProcedure(procParams, cancellationToken))
                .Then(tuple => new GetVotingCountryPointsAverageRankingsResponse(tuple.Item1, filtering, tuple.Item2));
        }
    }
}
