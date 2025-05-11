using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ErrorOr;
using Eurocentric.Domain.Enums;
using Eurocentric.Features.PublicApi.V0.Common;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.FakeRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.VotingCountryRankings;

public static class GetPointsShareVotingCountryRankings
{
    internal static IEndpointRouteBuilder MapGetPointsShareVotingCountryRankings(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("v0.2/voting-country-rankings/points-share", Endpoint.HandleAsync)
            .WithName("PublicApi.V0.2.GetPointsShareVotingCountryRankings")
            .WithSummary("Get points share voting country rankings")
            .WithDescription("Ranks all voting countries by the sum total points they have given to a specified competing " +
                             "country, as a share of the maximum available points, and returns a page of rankings.")
            .WithTags(EndpointTags.VotingCountryRankings)
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return apiGroup;
    }

    private static IEnumerable<FakeVoterPointsDatum> FilterByCompetingCountryCode(this IEnumerable<FakeVoterPointsDatum> data,
        string competingCountryCode) => data.Where(d => d.CompetingCountryCode == competingCountryCode);

    private static IEnumerable<FakeVoterPointsDatum> FilterByContestYear(this IEnumerable<FakeVoterPointsDatum> data,
        int? minYear, int? maxYear)
    {
        int min = minYear ?? int.MinValue;
        int max = maxYear ?? int.MaxValue;

        return data.Where(d => d.ContestYear >= min && d.ContestYear <= max);
    }

    private static IEnumerable<FakeVoterPointsDatum> FilterByContestStages(this IEnumerable<FakeVoterPointsDatum> data,
        ContestStages contestStages) => contestStages switch
    {
        ContestStages.All => data,
        ContestStages.SemiFinal1 => data.Where(d => d.ContestStage == ContestStage.SemiFinal1),
        ContestStages.SemiFinal2 => data.Where(d => d.ContestStage == ContestStage.SemiFinal2),
        ContestStages.SemiFinals => data.Where(d => d.ContestStage is ContestStage.SemiFinal1 or ContestStage.SemiFinal2),
        ContestStages.GrandFinal => data.Where(d => d.ContestStage == ContestStage.GrandFinal),
        _ => throw new InvalidEnumArgumentException(nameof(contestStages), (int)contestStages, typeof(ContestStages))
    };

    public sealed record Ranking(int Rank, string CountryCode, string CountryName, double PointsShare);

    public sealed record FilteringMetadata(
        string CompetingCountryCode,
        int? MinYear,
        int? MaxYear,
        ContestStages ContestStages,
        VotingMethod VotingMethod);

    public sealed record Response(Ranking[] Rankings, FilteringMetadata Filtering, PaginationMetadata Pagination);

    public sealed record QueryParams
    {
        [FromQuery(Name = "competingCountryCode")]
        [Required]
        public required string CountryCode { get; init; }

        [FromQuery(Name = "minYear")]
        public int? MinYear { get; init; }

        [FromQuery(Name = "maxYear")]
        public int? MaxYear { get; init; }

        [FromQuery(Name = "contestStages")]
        [DefaultValue(typeof(ContestStages), "All")]
        public ContestStages? ContestStages { get; init; }

        [FromQuery(Name = "votingMethod")]
        [DefaultValue(typeof(VotingMethod), "Any")]
        public VotingMethod? VotingMethod { get; init; }

        [FromQuery(Name = "pageIndex")]
        public int? PageIndex { get; init; }

        [FromQuery(Name = "pageSize")]
        public int? PageSize { get; init; }

        [FromQuery(Name = "descending")]
        public bool? Descending { get; init; }
    }

    internal sealed record Query(
        string CompetingCountryCode,
        int? MinYear,
        int? MaxYear,
        ContestStages ContestStages,
        VotingMethod VotingMethod,
        int PageIndex,
        int PageSize,
        bool Descending) : IQuery<Response>;

    internal sealed class Handler(FakeVoterPointsDataRepository repository) : IQueryHandler<Query, Response>
    {
        public async Task<ErrorOr<Response>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            IEnumerable<FakeVoterPointsDatum> data = GetData(query.VotingMethod)
                .FilterByCompetingCountryCode(query.CompetingCountryCode)
                .FilterByContestYear(query.MinYear, query.MaxYear)
                .FilterByContestStages(query.ContestStages);

            Ranking[] fullRankings = data.GroupBy(d => new
                {
                    CountryCode = d.VotingCountryCode, CountryName = d.VotingCountryName
                })
                .Select(group => new
                {
                    group.Key.CountryCode,
                    group.Key.CountryName,
                    PointsShare = group.Sum(d => d.PointsAwarded)
                                  / (1.0 * group.Sum(d => d.AvailablePoints))
                })
                .OrderByDescending(item => item.PointsShare)
                .Select((item, index) => new Ranking(index + 1, item.CountryCode, item.CountryName, item.PointsShare))
                .ToArray();

            Ranking[] rankings = query.Descending
                ? fullRankings.OrderByDescending(ranking => ranking.PointsShare)
                    .Skip(query.PageIndex * query.PageSize)
                    .Take(query.PageSize)
                    .ToArray()
                : fullRankings
                    .Skip(query.PageIndex * query.PageSize)
                    .Take(query.PageSize)
                    .ToArray();

            FilteringMetadata filtering = new(query.CompetingCountryCode,
                query.MinYear,
                query.MaxYear,
                query.ContestStages,
                query.VotingMethod);

            PaginationMetadata pagination = PaginationMetadata.Create(fullRankings.Length,
                query.PageIndex,
                query.PageSize,
                query.Descending);

            return ErrorOrFactory.From(new Response(rankings, filtering, pagination));
        }

        private IEnumerable<FakeVoterPointsDatum> GetData(VotingMethod votingMethod) => votingMethod switch
        {
            VotingMethod.Any => repository.GetTelevoteData().Concat(repository.GetJuryData()),
            VotingMethod.Jury => repository.GetJuryData(),
            VotingMethod.Televote => repository.GetTelevoteData(),
            _ => throw new InvalidEnumArgumentException(nameof(votingMethod), (int)votingMethod, typeof(VotingMethod))
        };
    }

    private static class Endpoint
    {
        internal static async Task<Ok<Response>> HandleAsync([AsParameters] QueryParams queryParams,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default)
        {
            ErrorOr<Response> errorsOrResponse = await InitializeQuery(queryParams)
                .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken));

            return TypedResults.Ok(errorsOrResponse.Value);
        }

        private static ErrorOr<Query> InitializeQuery(QueryParams queryParams) => ErrorOrFactory.From(new Query(
            queryParams.CountryCode,
            queryParams.MinYear,
            queryParams.MaxYear,
            queryParams.ContestStages.GetValueOrDefault(),
            queryParams.VotingMethod.GetValueOrDefault(),
            queryParams.PageIndex.GetValueOrDefault(PaginationDefaults.PageIndex),
            queryParams.PageSize.GetValueOrDefault(PaginationDefaults.PageSize),
            queryParams.Descending.GetValueOrDefault(PaginationDefaults.Descending)));
    }
}
