using Dapper;
using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Common.DataAccess;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.PublicApi.V0.Common.Mappings;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.Common;
using Eurocentric.Infrastructure.DataAccess.Dapper;
using Microsoft.AspNetCore.Http;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.Rankings.GetCompetingCountryPointsAverageRankings;

internal static class GetCompetingCountryPointsAverageRankingsFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [AsParameters] GetCompetingCountryPointsAverageRankingsRequest queryParams,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<GetCompetingCountryPointsAverageRankingsResponse> errorsOrResponse =
            await bus.Send(queryParams.ToQuery(), cancellationToken: cancellationToken);

        return TypedResults.Ok(errorsOrResponse.Value);
    }

    private static Query ToQuery(this GetCompetingCountryPointsAverageRankingsRequest queryParams) => new()
    {
        PageIndex = queryParams.PageIndex.GetValueOrDefault(QueryParamDefaults.PageIndex),
        PageSize = queryParams.PageSize.GetValueOrDefault(QueryParamDefaults.PageSize),
        Descending = queryParams.Descending.GetValueOrDefault(QueryParamDefaults.Descending),
        VotingMethod = queryParams.VotingMethod.GetValueOrDefault(QueryParamDefaults.VotingMethod),
        ContestStage = queryParams.ContestStage.GetValueOrDefault(QueryParamDefaults.ContestStage),
        MinYear = queryParams.MinYear,
        MaxYear = queryParams.MaxYear,
        VotingCountryCode = queryParams.VotingCountryCode
    };

    private static CompetingCountryPointsAverageFilters ToFilters(this Query query) => new()
    {
        VotingMethod = query.VotingMethod,
        ContestStage = query.ContestStage,
        MinYear = query.MinYear,
        MaxYear = query.MaxYear,
        VotingCountryCode = query.VotingCountryCode
    };

    private static DynamicParameters ToDynamicParameters(this Query query)
    {
        DynamicParameters dynamicParameters = new();

        return dynamicParameters.AddPaginationInputParams(query.PageIndex, query.PageSize, query.Descending)
            .AddTotalItemsOutputParam()
            .AddContestStagesInputParams(query.ContestStage)
            .AddContestYearInputParams(query.MinYear, query.MaxYear)
            .AddVotingMethodInputParams(query.VotingMethod)
            .AddVotingCountryCodeInputParam(query.VotingCountryCode);
    }

    internal sealed record Query : PaginatedQuery, IQuery<GetCompetingCountryPointsAverageRankingsResponse>
    {
        public required QueryableVotingMethod VotingMethod { get; init; }

        public required QueryableContestStage ContestStage { get; init; }

        public required int? MinYear { get; init; }

        public required int? MaxYear { get; init; }

        public required string? VotingCountryCode { get; init; }
    }

    internal sealed class QueryHandler(IDbStoredProcedureRunner procRunner)
        : IQueryHandler<Query, GetCompetingCountryPointsAverageRankingsResponse>
    {
        public async Task<ErrorOr<GetCompetingCountryPointsAverageRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken)
        {
            DynamicParameters dynamicParameters = query.ToDynamicParameters();

            CompetingCountryPointsAverageRanking[] items =
                await procRunner.ExecuteAsync<CompetingCountryPointsAverageRanking>(
                    DbStoredProcedures.V0.GetCompetingCountryPointsAverageRankings,
                    dynamicParameters,
                    cancellationToken);

            return new GetCompetingCountryPointsAverageRankingsResponse
            {
                Rankings = items,
                Filters = query.ToFilters(),
                Pagination = query.ToPaginationMetadataWithTotalItems(dynamicParameters.GetTotalItemsOutputParamValue())
            };
        }
    }
}
