using System.ComponentModel.DataAnnotations;
using ErrorOr;
using Eurocentric.Apis.Public.V0.Constants;
using Eurocentric.Apis.Public.V0.Dtos.Rankings.Common;
using Eurocentric.Apis.Public.V0.Enums;
using Eurocentric.Domain.V0Analytics.Rankings.CompetingCountries;
using Eurocentric.Infrastructure.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using RankingDto = Eurocentric.Apis.Public.V0.Dtos.Rankings.CompetingCountries.CompetingCountryPointsInRangeRanking;
using MetadataDto = Eurocentric.Apis.Public.V0.Dtos.Rankings.CompetingCountries.CompetingCountryPointsInRangeMetadata;

namespace Eurocentric.Apis.Public.V0.Features.Rankings.CompetingCountries;

public static class GetCompetingCountryPointsInRangeRankingsV0Point2
{
    internal static IEndpointRouteBuilder MapGetCompetingCountryPointsInRangeRankingsV0Point2(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("v0.2/rankings/competing-countries/points-in-range", ExecuteAsync)
            .WithName("PublicApi.V0.2.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings")
            .WithTags(V0Group.CompetingCountryRankings.Tag)
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status422UnprocessableEntity);

        return builder;
    }

    private static async Task<IResult> ExecuteAsync(
        [AsParameters] Request request,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<Response> errorsOrResponse = await bus.Send(request, cancellationToken: cancellationToken);

        return TypedResults.Ok(errorsOrResponse.Value);
    }

    private static PointsInRangeQuery MapToPointsInRangeQuery(this Request request) => new()
    {
        MinPoints = request.MinPoints,
        MaxPoints = request.MaxPoints,
        MinYear = request.MinYear,
        MaxYear = request.MaxYear,
        ContestStages = request.ContestStages.MapToNullableDomainContestStages(),
        VotingMethod = request.VotingMethod?.MapToDomainVotingMethod(),
        VotingCountryCode = request.VotingCountryCode,
        PageIndex = request.PageIndex.GetValueOrDefault(0),
        PageSize = request.PageSize.GetValueOrDefault(10),
        Descending = request.Descending.GetValueOrDefault(false)
    };

    private static RankingDto MapToRankingDto(PointsInRangeRanking ranking) => new()
    {
        Rank = ranking.Rank,
        CountryCode = ranking.CountryCode,
        CountryName = ranking.CountryName,
        PointsInRange = ranking.PointsInRange,
        PointsAwardsInRange = ranking.PointsAwardsInRange,
        PointsAwards = ranking.PointsAwards,
        Broadcasts = ranking.Broadcasts,
        Contests = ranking.Contests,
        VotingCountries = ranking.VotingCountries
    };

    private static MetadataDto MapToMetadataDto(this PointsInRangeMetadata metadata) => new()
    {
        MinPoints = metadata.MinPoints,
        MaxPoints = metadata.MaxPoints,
        MinYear = metadata.MinYear,
        MaxYear = metadata.MaxYear,
        ContestStages = metadata.ContestStages?.Select(stage => stage.MapToApiContestStage()).ToArray(),
        VotingMethod = metadata.VotingMethod?.MapToApiVotingMethod(),
        VotingCountryCode = metadata.VotingCountryCode,
        PageIndex = metadata.PageIndex,
        PageSize = metadata.PageSize,
        Descending = metadata.Descending,
        TotalRankings = metadata.TotalRankings,
        TotalPages = metadata.TotalPages
    };

    public sealed record Request : PaginatedRequest, IQuery<Response>
    {
        [FromQuery(Name = "minPoints")]
        [Required]
        public required int MinPoints { get; init; }

        [FromQuery(Name = "maxPoints")]
        [Required]
        public required int MaxPoints { get; init; }

        [FromQuery(Name = "minYear")]
        public int? MinYear { get; init; }

        [FromQuery(Name = "maxYear")]
        public int? MaxYear { get; init; }

        [FromQuery(Name = "contestStage")]
        public ContestStage[]? ContestStages { get; init; }

        [FromQuery(Name = "votingMethod")]
        public VotingMethod? VotingMethod { get; init; }

        [FromQuery(Name = "votingCountryCode")]
        public string? VotingCountryCode { get; init; }
    }

    public sealed record Response(RankingDto[] Rankings, MetadataDto Metadata);

    [UsedImplicitly]
    internal sealed class QueryHandler(ICompetingCountryRankingsGateway gateway) : IQueryHandler<Request, Response>
    {
        public async Task<ErrorOr<Response>> OnHandle(Request query, CancellationToken cancellationToken)
        {
            ErrorOr<PointsInRangeRankingsPage> errorsOrValue =
                await gateway.GetPointsInRangeRankingsPageAsync(query.MapToPointsInRangeQuery(), cancellationToken);

            if (errorsOrValue.IsError)
            {
                return errorsOrValue.FirstError;
            }

            (PointsInRangeRanking[] rankings, PointsInRangeMetadata metadata) = errorsOrValue.Value;

            return new Response(rankings.Select(MapToRankingDto).ToArray(), metadata.MapToMetadataDto());
        }
    }
}
