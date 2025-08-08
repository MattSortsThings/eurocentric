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
            .WithDescription("Retrieves a single broadcast")
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
            .WithDescription("Deletes a single existing broadcast")
            .HasApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
