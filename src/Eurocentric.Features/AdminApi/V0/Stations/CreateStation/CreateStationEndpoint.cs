using ErrorOr;
using Eurocentric.Features.Shared.ErrorHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Stations.CreateStation;

internal static class CreateStationEndpoint
{
    private const string V0Point2Route = "v0.2/stations";

    private static Delegate Handler => static async ([FromBody] CreateStationCommand request,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await ErrorOrFactory.From(request)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToResultOrProblemAsync(MapToCreatedAtRoute);

    internal static void MapCreateStation(this IEndpointRouteBuilder api) => api.MapPost(V0Point2Route, Handler)
        .WithName("CreateStationV0.2")
        .WithSummary("Create a station")
        .WithDescription("Creates a new station from parameters supplied in the request body.")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithTags("Stations");

    private static CreatedAtRoute<CreateStationResponse> MapToCreatedAtRoute(CreateStationResponse response) =>
        TypedResults.CreatedAtRoute(response,
            "GetStationV0.2",
            new RouteValueDictionary { ["stationId"] = response.Station.Id });
}
