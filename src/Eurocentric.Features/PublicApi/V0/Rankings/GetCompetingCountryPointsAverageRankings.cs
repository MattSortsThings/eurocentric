using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Dapper;
using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.PublicApi.V0.Common.Extensions;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.Constants;
using Eurocentric.Infrastructure.DataAccess.Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.Rankings;

public sealed record GetCompetingCountryPointsAverageRankingsRequest : PaginatedQueryParameters
{
    [FromQuery(Name = "minYear")]
    [Description("Inclusive minimum contest year")]
    public int? MinYear { get; init; }

    [FromQuery(Name = "maxYear")]
    [Description("Inclusive maximum contest year")]
    public int? MaxYear { get; init; }

    [FromQuery(Name = "contestStage")]
    [DefaultValue(typeof(ContestStageFilter), nameof(ContestStageFilter.Any))]
    [Description("Filters the queryable data by contest stage")]
    public ContestStageFilter? ContestStage { get; init; }

    [FromQuery(Name = "votingMethod")]
    [DefaultValue(typeof(VotingMethodFilter), nameof(VotingMethodFilter.Any))]
    [Description("Filters the queryable data by voting method")]
    public VotingMethodFilter? VotingMethod { get; init; }

    [FromQuery(Name = "votingCountryCode")]
    [RegularExpression("^[A-Za-z]{2}$")]
    [Description("Filters the queryable data by voting country code")]
    public string? VotingCountryCode { get; init; }
}

public sealed record GetCompetingCountryPointsAverageRankingsResponse
{
    public required CompetingCountryPointsAverageRanking[] Rankings { get; init; }

    public required CompetingCountryPointsAverageFilters Filters { get; init; }

    public required PaginationInfo Pagination { get; init; }
}

public sealed record CompetingCountryPointsAverageRanking
{
    public required int Rank { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required double PointsAverage { get; init; }

    public required int PointsAwards { get; init; }

    public required int Broadcasts { get; init; }

    public required int Contests { get; init; }

    public required int VotingCountries { get; init; }
}

public sealed record CompetingCountryPointsAverageFilters
{
    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public required ContestStageFilter ContestStage { get; init; }

    public required VotingMethodFilter VotingMethod { get; init; }

    public string? VotingCountryCode { get; init; }
}

internal static class GetCompetingCountryPointsAverageRankings
{
    internal static IEndpointRouteBuilder MapGetCompetingCountryPointsAverageRankings(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("rankings/competing-countries/points-average", ExecuteAsync)
            .WithName(EndpointNames.Rankings.GetCompetingCountryPointsAverageRankings)
            .WithSummary("Get competing country points average rankings")
            .WithDescription("Ranks every competing country by the average value of all the individual points awards " +
                             "it has received. Returns a page of rankings.")
            .WithTags(EndpointTags.Rankings)
            .HasApiVersion(0, 2)
            .Produces<GetCompetingCountryPointsAverageRankingsResponse>();

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetCompetingCountryPointsAverageRankingsResponse>>> ExecuteAsync(
        [AsParameters] GetCompetingCountryPointsAverageRankingsRequest queryParameters,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<GetCompetingCountryPointsAverageRankingsResponse> errorsOrResponse =
            await bus.Send(queryParameters.ToQuery(), cancellationToken: cancellationToken);

        return TypedResults.Ok(errorsOrResponse.Value);
    }

    private static Query ToQuery(this GetCompetingCountryPointsAverageRankingsRequest request) => new()
    {
        MinYear = request.MinYear,
        MaxYear = request.MaxYear,
        ContestStage = request.ContestStage.GetValueOrDefault(QueryParameterDefaults.ContestStage),
        VotingMethod = request.VotingMethod.GetValueOrDefault(QueryParameterDefaults.VotingMethod),
        VotingCountryCode = request.VotingCountryCode?.ToUpperInvariant(),
        PageIndex = request.PageIndex.GetValueOrDefault(QueryParameterDefaults.PageIndex),
        PageSize = request.PageSize.GetValueOrDefault(QueryParameterDefaults.PageSize),
        Descending = request.Descending.GetValueOrDefault(QueryParameterDefaults.Descending)
    };

    private static DynamicParameters ToDynamicParameters(this Query query)
    {
        DynamicParameters dynamicParameters = new();

        dynamicParameters.AddContestStagesInputParameter(query.ContestStage)
            .AddContestYearInputParameters(query.MinYear, query.MaxYear)
            .AddPaginationInputParameters(query.PageIndex, query.PageSize, query.Descending)
            .AddVotingCountryCodeInputParameter(query.VotingCountryCode)
            .AddVotingMethodInputParameters(query.VotingMethod);

        return dynamicParameters;
    }

    private static CompetingCountryPointsAverageFilters ToFilters(this Query query) => new()
    {
        MinYear = query.MinYear,
        MaxYear = query.MaxYear,
        ContestStage = query.ContestStage,
        VotingMethod = query.VotingMethod,
        VotingCountryCode = query.VotingCountryCode
    };

    internal sealed record Query : IQuery<GetCompetingCountryPointsAverageRankingsResponse>
    {
        public int? MinYear { get; init; }

        public int? MaxYear { get; init; }

        public ContestStageFilter ContestStage { get; init; }

        public VotingMethodFilter VotingMethod { get; init; }

        public string? VotingCountryCode { get; init; }

        public int PageIndex { get; init; }

        public int PageSize { get; init; }

        public bool Descending { get; init; }
    }

    internal sealed class Handler(IDbConnectionFactory dbConnectionFactory)
        : IQueryHandler<Query, GetCompetingCountryPointsAverageRankingsResponse>
    {
        public async Task<ErrorOr<GetCompetingCountryPointsAverageRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken)
        {
            const string storedProcedureName = StoredProcedures.GetCompetingCountryPointsAverageRankings;
            DynamicParameters dynamicParameters = query.ToDynamicParameters();

            using IDbConnection dbConnection = await dbConnectionFactory.CreateConnectionAsync(cancellationToken);

            (PaginationInfo pagination, CompetingCountryPointsAverageRanking[] rankings) =
                await dbConnection.QueryPagedAsync<CompetingCountryPointsAverageRanking>(storedProcedureName, dynamicParameters);

            return ErrorOrFactory.From(new GetCompetingCountryPointsAverageRankingsResponse
            {
                Rankings = rankings, Filters = query.ToFilters(), Pagination = pagination
            });
        }
    }
}
