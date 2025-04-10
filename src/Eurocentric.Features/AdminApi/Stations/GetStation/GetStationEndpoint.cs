using ErrorOr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.Stations.GetStation;

internal static class GetStationEndpoint
{
    private const string V0Point1Route = "v0.1/stations/{stationId:int}";
    private const string V0Point2Route = "v0.2/stations/{stationId:int}";

    private static Delegate Handler => static async ([FromRoute] int stationId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await ErrorOrFactory.From(new GetStationQuery(stationId))
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .MatchFirst<GetStationResponse, IResult>(TypedResults.Ok, _ => TypedResults.NotFound());

    internal static void MapGetStation(this IEndpointRouteBuilder api)
    {
        api.MapGet(V0Point1Route, Handler)
            .WithName("GetStationV0.1")
            .WithSummary("Get a station")
            .WithDescription("Retrieves a single station. The station ID must be supplied as a route parameter.")
            .Produces<GetStationResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Stations");

        api.MapGet(V0Point2Route, Handler)
            .WithName("GetStationV0.2")
            .WithSummary("Get a station")
            .WithDescription("Retrieves a single station. The station ID must be supplied as a route parameter.")
            .Produces<GetStationResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Stations");
    }
}
