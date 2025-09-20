using System.ComponentModel.DataAnnotations;
using ErrorOr;
using Eurocentric.Apis.Public.V0.Constants;
using Eurocentric.Apis.Public.V0.Enums;
using Eurocentric.Apis.Public.V0.Versioning;
using Eurocentric.Domain.V0Analytics.Scoreboard;
using Eurocentric.Infrastructure.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using ScoreboardMetadata = Eurocentric.Domain.V0Analytics.Scoreboard.ScoreboardMetadata;
using ScoreboardRow = Eurocentric.Domain.V0Analytics.Scoreboard.ScoreboardRow;
using ScoreboardMetadataDto = Eurocentric.Apis.Public.V0.Dtos.Scoreboards.ScoreboardMetadata;
using ScoreboardRowDto = Eurocentric.Apis.Public.V0.Dtos.Scoreboards.ScoreboardRow;

namespace Eurocentric.Apis.Public.V0.Features.Scoreboards;

public static class GetScoreboard
{
    internal static IEndpointRouteBuilder MapGetScoreboard(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/scoreboards", ExecuteAsync)
            .WithName(V0Group.Scoreboards.Endpoints.GetScoreboard)
            .IntroducedInV0Point2()
            .WithTags(V0Group.Scoreboards.Tag)
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

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

    private static Response MapToResponse(Scoreboard scoreboard)
    {
        (ScoreboardRow[] rows, ScoreboardMetadata metadata) = scoreboard;

        ScoreboardRowDto[] scoreboardRows = rows.Select(MapToScoreboardRowDto).ToArray();
        ScoreboardMetadataDto scoreboardMetadata = metadata.MapToScoreboardMetadataDto();

        return new Response(scoreboardRows, scoreboardMetadata);
    }

    private static ScoreboardQuery MapToScoreboardQuery(Request request) => new()
    {
        ContestYear = request.ContestYear, ContestStage = request.ContestStage.MapToDomainContestStage()
    };

    private static ScoreboardMetadataDto MapToScoreboardMetadataDto(this ScoreboardMetadata metadata) => new()
    {
        ContestYear = metadata.ContestYear,
        ContestStage = metadata.ContestStage.MapToApiContestStage(),
        TelevoteOnlyBroadcast = metadata.TelevoteOnlyBroadcast
    };

    private static ScoreboardRowDto MapToScoreboardRowDto(ScoreboardRow row) => new()
    {
        FinishingPosition = row.FinishingPosition,
        RunningOrderSpot = row.RunningOrderSpot,
        CountryCode = row.CountryCode,
        CountryName = row.CountryName,
        ActName = row.ActName,
        SongTitle = row.SongTitle,
        JuryPoints = row.JuryPoints,
        TelevotePoints = row.TelevotePoints,
        OverallPoints = row.OverallPoints
    };

    public sealed record Request : IQuery<Response>
    {
        [FromQuery(Name = "contestYear")]
        [Required]
        public required int ContestYear { get; init; }

        [FromQuery(Name = "contestStage")]
        [Required]
        public required ContestStage ContestStage { get; init; }
    }

    public sealed record Response(ScoreboardRowDto[] ScoreboardRows, ScoreboardMetadataDto Metadata);

    [UsedImplicitly]
    internal sealed class QueryHandler(IScoreboardGateway gateway) : IQueryHandler<Request, Response>
    {
        public async Task<ErrorOr<Response>> OnHandle(Request query, CancellationToken cancellationToken) => await query
            .ToErrorOr()
            .Then(MapToScoreboardQuery)
            .ThenAsync(scoreboardQuery => gateway.GetScoreboardAsync(scoreboardQuery, cancellationToken))
            .Then(MapToResponse);
    }
}
