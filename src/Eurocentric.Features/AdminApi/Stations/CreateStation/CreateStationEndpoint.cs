using ErrorOr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.Stations.CreateStation;

internal static class CreateStationEndpoint
{
    private const string V0Point2Route = "v0.2/stations";

    private static Delegate Handler => static async ([FromBody] CreateStationCommand request,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await ErrorOrFactory.From(request)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .MatchFirst<CreateStationResponse, IResult>(response =>
                TypedResults.CreatedAtRoute(response,
                    "GetStationV0.2",
                    new RouteValueDictionary { ["stationId"] = response.Station.Id }),
            _ => TypedResults.BadRequest());

    internal static void MapCreateStation(this IEndpointRouteBuilder api) => api.MapPost(V0Point2Route, Handler)
        .WithName("CreateStationV0.2")
        .WithSummary("Create a station")
        .WithDescription("Creates a new station from parameters supplied in the request body.")
        .Produces<CreateStationResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithTags("Stations");
}
