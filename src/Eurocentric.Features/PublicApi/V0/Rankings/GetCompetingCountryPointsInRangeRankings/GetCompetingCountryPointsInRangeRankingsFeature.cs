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

namespace Eurocentric.Features.PublicApi.V0.Rankings.GetCompetingCountryPointsInRangeRankings;

internal static class GetCompetingCountryPointsInRangeRankingsFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [AsParameters] GetCompetingCountryPointsInRangeRankingsRequest queryParams,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await bus.SendWithResponseMapperAsync(queryParams.ToQuery(),
        TypedResults.Ok,
        cancellationToken);

    private static Query ToQuery(this GetCompetingCountryPointsInRangeRankingsRequest queryParams) => new()
    {
        PageIndex = queryParams.PageIndex.GetValueOrDefault(QueryParamDefaults.PageIndex),
        PageSize = queryParams.PageSize.GetValueOrDefault(QueryParamDefaults.PageSize),
        Descending = queryParams.Descending.GetValueOrDefault(QueryParamDefaults.Descending),
        VotingMethod = queryParams.VotingMethod.GetValueOrDefault(QueryParamDefaults.VotingMethod),
        ContestStage = queryParams.ContestStage.GetValueOrDefault(QueryParamDefaults.ContestStage),
        MinYear = queryParams.MinYear,
        MaxYear = queryParams.MaxYear,
        VotingCountryCode = queryParams.VotingCountryCode,
        MinPoints = queryParams.MinPoints,
        MaxPoints = queryParams.MaxPoints
    };

    private static CompetingCountryPointsInRangeFilters ToFilters(this Query query) => new()
    {
        MinPoints = query.MinPoints,
        MaxPoints = query.MaxPoints,
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
            .AddVotingCountryCodeInputParam(query.VotingCountryCode)
            .AddPointsValueInputParams(query.MinPoints, query.MaxPoints);
    }

    internal sealed record Query : PaginatedQuery, IQuery<GetCompetingCountryPointsInRangeRankingsResponse>
    {
        public required int MinPoints { get; init; }

        public required int MaxPoints { get; init; }

        public required QueryableVotingMethod VotingMethod { get; init; }

        public required QueryableContestStage ContestStage { get; init; }

        public required int? MinYear { get; init; }

        public required int? MaxYear { get; init; }

        public required string? VotingCountryCode { get; init; }
    }

    internal sealed class QueryHandler(IDbStoredProcedureRunner procRunner)
        : IQueryHandler<Query, GetCompetingCountryPointsInRangeRankingsResponse>
    {
        public async Task<ErrorOr<GetCompetingCountryPointsInRangeRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken)
        {
            DynamicParameters dynamicParameters = query.ToDynamicParameters();

            CompetingCountryPointsInRangeRanking[] items =
                await procRunner.ExecuteAsync<CompetingCountryPointsInRangeRanking>(
                    DbStoredProcedures.V0.GetCompetingCountryPointsInRangeRankings,
                    dynamicParameters,
                    cancellationToken);

            return new GetCompetingCountryPointsInRangeRankingsResponse
            {
                Rankings = items,
                Filters = query.ToFilters(),
                Pagination = query.ToPaginationMetadataWithTotalItems(dynamicParameters.GetTotalItemsOutputParamValue())
            };
        }
    }
}
