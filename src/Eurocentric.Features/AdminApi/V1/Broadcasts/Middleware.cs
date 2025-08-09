using Eurocentric.Features.AdminApi.V1.Broadcasts.AwardJuryPoints;
using Eurocentric.Features.AdminApi.V1.Broadcasts.AwardTelevotePoints;
using Eurocentric.Features.AdminApi.V1.Broadcasts.DeleteBroadcast;
using Eurocentric.Features.AdminApi.V1.Broadcasts.GetBroadcast;
using Eurocentric.Features.AdminApi.V1.Broadcasts.GetBroadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the endpoints tagged with "Broadcasts".
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapBroadcastsEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("broadcasts")
            .WithTags(EndpointNames.Tags.Broadcasts);

        group.MapGet("/{broadcastId:guid}", GetBroadcastFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Broadcasts.GetBroadcast)
            .WithSummary("Get a broadcast")
            .WithDescription("Retrieves a single broadcast.")
            .HasApiVersion(1, 0)
            .Produces<GetBroadcastResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/", GetBroadcastsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Broadcasts.GetBroadcasts)
            .WithSummary("Get all broadcasts")
            .WithDescription("Retrieves a list of all existing broadcasts, in broadcast date order.")
            .HasApiVersion(1, 0)
            .Produces<GetBroadcastsResponse>();

        group.MapDelete("/{broadcastId:guid}", DeleteBroadcastFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Broadcasts.DeleteBroadcast)
            .WithSummary("Delete a broadcast")
            .WithDescription("Deletes a single existing broadcast.")
            .HasApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPatch("/{broadcastId:guid}/award-jury", AwardJuryPointsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Broadcasts.AwardJuryPoints)
            .WithSummary("Award jury points in a broadcast")
            .WithDescription("Awards the points from a jury to the competitors in an existing broadcast.")
            .HasApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        group.MapPatch("/{broadcastId:guid}/award-televote", AwardTelevotePointsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Broadcasts.AwardTelevotePoints)
            .WithSummary("Award televote points in a broadcast")
            .WithDescription("Awards the points from a televote to the competitors in an existing broadcast.")
            .HasApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
