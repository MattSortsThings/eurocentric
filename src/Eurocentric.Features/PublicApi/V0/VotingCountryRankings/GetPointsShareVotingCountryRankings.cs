using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ErrorOr;
using Eurocentric.Domain.Placeholders;
using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Common.Dtos;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.PublicApi.V0.Common.QueryHelpers;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.InMemoryRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.VotingCountryRankings;

public sealed record PointsShareVotingCountryRanking
{
    public required int Rank { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required int PointsAwards { get; init; }

    public required int TotalPoints { get; init; }

    public required int AvailablePoints { get; init; }

    public required decimal PointsShare { get; init; }
}

public sealed record PointsShareVotingCountryRankingFilters
{
    public string CompetingCountryCode { get; init; } = string.Empty;

    public VotingMethod VotingMethod { get; init; }

    public int? StartYear { get; init; }

    public int? EndYear { get; init; }

    public ContestStages ContestStages { get; init; }
}

public sealed record GetPointsShareVotingCountryRankingsResponse(
    PointsShareVotingCountryRanking[] Rankings,
    PointsShareVotingCountryRankingFilters Filters,
    PaginationMetadata Pagination);

public sealed record GetPointsShareVotingCountryRankingsQueryParams : PaginatedQueryParamsBase
{
    [Required]
    [FromQuery(Name = "competingCountryCode")]
    public required string CompetingCountryCode { get; init; }

    [FromQuery(Name = "votingMethod")]
    [DefaultValue(typeof(VotingMethod), "Any")]
    public VotingMethod? VotingMethod { get; init; }

    [FromQuery(Name = "startYear")]
    public int? StartYear { get; init; }

    [FromQuery(Name = "endYear")]
    public int? EndYear { get; init; }

    [FromQuery(Name = "contestStages")]
    [DefaultValue(typeof(ContestStages), "All")]
    public ContestStages? ContestStages { get; init; }
}

internal static class GetPointsShareVotingCountryRankings
{
    internal static IEndpointRouteBuilder MapGetPointsShareVotingCountryRankings(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("voting-country-rankings/points-share", HandleAsync)
            .WithName(EndpointIds.VotingCountryRankings.GetPointsShareVotingCountryRankings)
            .WithSummary("Get points share voting country rankings")
            .WithDescription("Ranks every voting country by the total points it has awarded to a given competing country, " +
                             "as a share of the maximum possible points, and returns a page of rankings.")
            .HasApiVersion(0, 2)
            .Produces<GetPointsShareVotingCountryRankingsResponse>()
            .WithTags(EndpointTags.VotingCountryRankings);

        return apiGroup;
    }

    private static async Task<IResult> HandleAsync(
        [AsParameters] GetPointsShareVotingCountryRankingsQueryParams queryParams,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery(queryParams)
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery(GetPointsShareVotingCountryRankingsQueryParams queryParams)
    {
        Query query = new(queryParams.CompetingCountryCode,
            queryParams.VotingMethod ?? VotingMethod.Any,
            queryParams.StartYear,
            queryParams.EndYear,
            queryParams.ContestStages ?? ContestStages.All,
            queryParams.PageIndex ?? 0,
            queryParams.PageSize ?? 10,
            queryParams.Descending ?? false);

        return ErrorOrFactory.From(query);
    }

    private static InterimRankingsInfo ToInterimRankings(this IQueryable<QueryablePointsAward> awards)
    {
        int totalRankings = awards.DistinctBy(award => award.VotingCountryCode).Count();

        IQueryable<InterimRanking> rankings = awards.GroupBy(award => award.VotingCountryCode)
            .Select(group => new
            {
                VotingCountryCode = group.Key,
                TotalPoints = group.Sum(award => award.PointsValue),
                MaxPossiblePoints = group.Sum(award => award.MaxPointsValue),
                PointsAwards = group.Count()
            }).Select(item => new
            {
                item.VotingCountryCode,
                item.TotalPoints,
                item.MaxPossiblePoints,
                item.PointsAwards,
                PointsShare = Math.Round((decimal)(double)item.TotalPoints / item.MaxPossiblePoints, 6)
            }).OrderByDescending(item => item.PointsShare)
            .Select((item, index) => new
            {
                item.VotingCountryCode,
                item.TotalPoints,
                item.MaxPossiblePoints,
                item.PointsAwards,
                item.PointsShare,
                Rank = index + 1
            }).GroupBy(item => item.PointsShare)
            .SelectMany((grouping, index) => grouping.Select(item => new InterimRanking
            {
                Rank = index + 1,
                VotingCountryCode = item.VotingCountryCode,
                PointsAwards = item.PointsAwards,
                TotalPoints = item.TotalPoints,
                MaxPossiblePoints = item.MaxPossiblePoints,
                PointsShare = item.PointsShare
            }));

        return new InterimRankingsInfo(rankings, totalRankings);
    }

    internal sealed record Query(
        string CompetingCountryCode,
        VotingMethod VotingMethod,
        int? StartYear,
        int? EndYear,
        ContestStages ContestStages,
        int PageIndex,
        int PageSize,
        bool Descending) : IQuery<GetPointsShareVotingCountryRankingsResponse>;

    internal sealed class Handler(InMemoryQueryableRepository repository)
        : IQueryHandler<Query, GetPointsShareVotingCountryRankingsResponse>
    {
        public async Task<ErrorOr<GetPointsShareVotingCountryRankingsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            PointsShareVotingCountryRankingFilters filters = new()
            {
                CompetingCountryCode = query.CompetingCountryCode,
                VotingMethod = query.VotingMethod,
                StartYear = query.StartYear,
                EndYear = query.EndYear,
                ContestStages = query.ContestStages
            };

            (IQueryable<InterimRanking> interimRankings, int totalRankings) = GetQueryablePointsAwards(filters)
                .ToInterimRankings();

            PaginationMetadata pagination = new()
            {
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                Descending = query.Descending,
                TotalItems = totalRankings,
                TotalPages = totalRankings % query.PageSize == 0
                    ? totalRankings / query.PageSize
                    : (totalRankings / query.PageSize) + 1
            };

            IOrderedQueryable<InterimRanking> orderedInterimRankings = query.Descending
                ? interimRankings.OrderByDescending(ranking => ranking.Rank).ThenBy(ranking => ranking.VotingCountryCode)
                : interimRankings.OrderBy(ranking => ranking.Rank).ThenBy(ranking => ranking.VotingCountryCode);

            PointsShareVotingCountryRanking[] pageOfRankings = orderedInterimRankings.Skip(query.PageIndex * query.PageSize)
                .Take(query.PageSize)
                .Join(repository.QueryableCountries,
                    ranking => ranking.VotingCountryCode,
                    country => country.CountryCode,
                    (ranking, country) => new PointsShareVotingCountryRanking
                    {
                        Rank = ranking.Rank,
                        CountryCode = country.CountryCode,
                        CountryName = country.CountryName,
                        PointsShare = ranking.PointsShare,
                        PointsAwards = ranking.PointsAwards,
                        TotalPoints = ranking.TotalPoints,
                        AvailablePoints = ranking.MaxPossiblePoints
                    })
                .ToArray();

            return ErrorOrFactory.From(new GetPointsShareVotingCountryRankingsResponse(pageOfRankings, filters, pagination));
        }

        private IQueryable<QueryablePointsAward> GetQueryablePointsAwards(PointsShareVotingCountryRankingFilters filters) =>
            GetQueryablePointsAwardsByVotingMethod(filters.VotingMethod)
                .FilterByCompetingCountryCode(filters.CompetingCountryCode)
                .FilterByStartYear(filters.StartYear)
                .FilterByEndYear(filters.EndYear)
                .FilterByContestStages(filters.ContestStages);

        private IQueryable<QueryablePointsAward> GetQueryablePointsAwardsByVotingMethod(VotingMethod votingMethod) =>
            votingMethod switch
            {
                VotingMethod.Any => repository.QueryableJuryPointsAwards.Concat(repository.QueryableTelevotePointsAwards)
                    .AsQueryable(),
                VotingMethod.Jury => repository.QueryableJuryPointsAwards.AsQueryable(),
                VotingMethod.Televote => repository.QueryableTelevotePointsAwards.AsQueryable(),
                _ => throw new InvalidEnumArgumentException(nameof(votingMethod), (int)votingMethod, typeof(VotingMethod))
            };
    }

    private sealed record InterimRanking
    {
        public int Rank { get; init; }

        public string VotingCountryCode { get; init; } = string.Empty;

        public int PointsAwards { get; init; }

        public int TotalPoints { get; init; }

        public int MaxPossiblePoints { get; init; }

        public decimal PointsShare { get; init; }
    }

    private sealed record InterimRankingsInfo(IQueryable<InterimRanking> InterimRankings, int TotalRankings);
}
