using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dapper;
using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.PublicApi.V0.Common.Extensions;
using Eurocentric.Features.Shared.Documentation;
using Eurocentric.Features.Shared.ErrorHandling;
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

public sealed record GetCompetingCountryPointsInRangeRankingsRequest : PaginatedQueryParametersBase
{
    [FromQuery(Name = "minPoints")]
    [Description("Inclusive minimum points award value")]
    public required int MinPoints { get; init; }

    [FromQuery(Name = "maxPoints")]
    [Description("Inclusive maximum points award value")]
    public required int MaxPoints { get; init; }

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

public sealed record GetCompetingCountryPointsInRangeRankingsResponse : PaginatedResponseBase
{
    public required CompetingCountryPointsInRangeRanking[] Rankings { get; init; }

    public required CompetingCountryPointsInRangeFilters Filters { get; init; }
}

public sealed record CompetingCountryPointsInRangeRanking : IExampleProvider<CompetingCountryPointsInRangeRanking>
{
    public required int Rank { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required double PointsInRange { get; init; }

    public required int PointsAwards { get; init; }

    public required int Broadcasts { get; init; }

    public required int Contests { get; init; }

    public required int VotingCountries { get; init; }

    public static CompetingCountryPointsInRangeRanking CreateExample() => new()
    {
        Rank = 1,
        CountryCode = "AT",
        CountryName = "Austria",
        PointsInRange = 0.25,
        PointsAwards = 100,
        Broadcasts = 4,
        Contests = 2,
        VotingCountries = 40
    };
}

public sealed record CompetingCountryPointsInRangeFilters : IExampleProvider<CompetingCountryPointsInRangeFilters>
{
    public required int MinPoints { get; init; }

    public required int MaxPoints { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public required ContestStageFilter ContestStage { get; init; }

    public required VotingMethodFilter VotingMethod { get; init; }

    public string? VotingCountryCode { get; init; }

    public static CompetingCountryPointsInRangeFilters CreateExample() => new()
    {
        MinPoints = QueryParameterExamples.MinPoints,
        MaxPoints = QueryParameterExamples.MaxPoints,
        MinYear = null,
        MaxYear = null,
        ContestStage = QueryParameterDefaults.ContestStage,
        VotingMethod = QueryParameterDefaults.VotingMethod,
        VotingCountryCode = null
    };
}

internal static class GetCompetingCountryPointsInRangeRankings
{
    internal static IEndpointRouteBuilder MapGetCompetingCountryPointsInRangeRankings(this IEndpointRouteBuilder v0Group)
    {
        v0Group.MapGet("rankings/competing-countries/points-in-range", ExecuteAsync)
            .WithName(EndpointNames.Rankings.GetCompetingCountryPointsInRangeRankings)
            .WithSummary("Get competing country points in range rankings")
            .WithDescription("Ranks every competing country by the relative frequency of all the individual points awards " +
                             "it has received that have a value in a specific range. Returns a page of rankings.")
            .WithTags(EndpointTags.Rankings)
            .HasApiVersion(0, 2)
            .Produces<GetCompetingCountryPointsInRangeRankingsResponse>();

        return v0Group;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetCompetingCountryPointsInRangeRankingsResponse>>> ExecuteAsync(
        [AsParameters] GetCompetingCountryPointsInRangeRankingsRequest queryParameters,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery(queryParameters)
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery(GetCompetingCountryPointsInRangeRankingsRequest request)
    {
        Query query = new()
        {
            MinPoints = request.MinPoints,
            MaxPoints = request.MaxPoints,
            MinYear = request.MinYear,
            MaxYear = request.MaxYear,
            ContestStage = request.ContestStage.GetValueOrDefault(QueryParameterDefaults.ContestStage),
            VotingMethod = request.VotingMethod.GetValueOrDefault(QueryParameterDefaults.VotingMethod),
            VotingCountryCode = request.VotingCountryCode?.ToUpperInvariant(),
            PageIndex = request.PageIndex.GetValueOrDefault(QueryParameterDefaults.PageIndex),
            PageSize = request.PageSize.GetValueOrDefault(QueryParameterDefaults.PageSize),
            Descending = request.Descending.GetValueOrDefault(QueryParameterDefaults.Descending)
        };

        return ErrorOrFactory.From(query);
    }

    private static DynamicParameters ToDynamicParameters(this Query query)
    {
        DynamicParameters dynamicParameters = new();

        dynamicParameters.AddContestStagesInputParameter(query.ContestStage)
            .AddContestYearInputParameters(query.MinYear, query.MaxYear)
            .AddPaginationInputParameters(query.PageIndex, query.PageSize, query.Descending)
            .AddPointsValueInputParameters(query.MinPoints, query.MaxPoints)
            .AddVotingCountryCodeInputParameter(query.VotingCountryCode)
            .AddVotingMethodInputParameters(query.VotingMethod);

        return dynamicParameters;
    }

    private static CompetingCountryPointsInRangeFilters ToFilters(this Query query) => new()
    {
        MinPoints = query.MinPoints,
        MaxPoints = query.MaxPoints,
        MinYear = query.MinYear,
        MaxYear = query.MaxYear,
        ContestStage = query.ContestStage,
        VotingMethod = query.VotingMethod,
        VotingCountryCode = query.VotingCountryCode
    };

    internal sealed record Query : IQuery<GetCompetingCountryPointsInRangeRankingsResponse>
    {
        public int MinPoints { get; init; }

        public int MaxPoints { get; init; }

        public int? MinYear { get; init; }

        public int? MaxYear { get; init; }

        public ContestStageFilter ContestStage { get; init; }

        public VotingMethodFilter VotingMethod { get; init; }

        public string? VotingCountryCode { get; init; }

        public int PageIndex { get; init; }

        public int PageSize { get; init; }

        public bool Descending { get; init; }
    }

    internal sealed class Handler(IDbStoredProcedureRunner dbProcRunner) :
        IQueryHandler<Query, GetCompetingCountryPointsInRangeRankingsResponse>
    {
        public async Task<ErrorOr<GetCompetingCountryPointsInRangeRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken)
        {
            const string storedProcedureName = StoredProcedures.GetCompetingCountryPointsInRangeRankings;
            DynamicParameters dynamicParameters = query.ToDynamicParameters();

            (PaginationInfo pagination, CompetingCountryPointsInRangeRanking[] rankings) =
                await dbProcRunner.ExecuteAsync<PaginationInfo, CompetingCountryPointsInRangeRanking>(
                    storedProcedureName, dynamicParameters, cancellationToken);

            return ErrorOrFactory.From(new GetCompetingCountryPointsInRangeRankingsResponse
            {
                Rankings = rankings, Filters = query.ToFilters(), Pagination = pagination
            });
        }
    }
}
