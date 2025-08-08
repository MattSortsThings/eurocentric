using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Contests.CreateChildBroadcast;
using Eurocentric.Features.AdminApi.V1.Contests.CreateContest;
using Eurocentric.Features.AdminApi.V1.Contests.GetContest;
using Eurocentric.Features.AdminApi.V1.Contests.GetContests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi.V1.Contests;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the endpoints tagged with "Contests".
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapContestsEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("contests")
            .WithTags(EndpointNames.Tags.Contests);

        group.MapPost("/", CreateContestFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Contests.CreateContest)
            .WithSummary("Create a contest")
            .WithDescription("Creates a new country.")
            .HasApiVersion(1, 0)
            .Produces<CreateContestResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        group.MapPost("/{contestId:guid}/broadcasts", CreateChildBroadcastFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Contests.CreateChildBroadcast)
            .WithSummary("Create a child broadcast")
            .WithDescription("Creates a new broadcast as a child of an existing contest.")
            .HasApiVersion(1, 0)
            .Produces<CreateChildBroadcastResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        group.MapGet("/{contestId:guid}", GetContestFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Contests.GetContest)
            .WithSummary("Get a contest")
            .WithDescription("Retrieves a single contest")
            .HasApiVersion(1, 0)
            .Produces<GetContestResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/", GetContestsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Contests.GetContests)
            .WithSummary("Get all contests")
            .WithDescription("Retrieves a list of all existing contests, in contest year order.")
            .HasApiVersion(1, 0)
            .Produces<GetContestsResponse>();
    }
}
