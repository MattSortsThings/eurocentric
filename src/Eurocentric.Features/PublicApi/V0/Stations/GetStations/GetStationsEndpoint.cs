using ErrorOr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.Stations.GetStations;

internal static class GetStationsEndpoint
{
    private const string V0Point1Route = "v0.1/stations";

    private static Delegate Handler => static async ([AsParameters] GetStationsQuery request,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await ErrorOrFactory.From(request)
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .MatchFirst<GetStationsResponse, IResult>(TypedResults.Ok, _ => TypedResults.BadRequest());

    internal static void MapGetStations(this IEndpointRouteBuilder api) => api.MapGet(V0Point1Route, Handler)
        .WithName("GetStationsV0.1")
        .WithSummary("Get stations")
        .WithDescription("Retrieves a list of stations matching the query string parameters.")
        .Produces<GetStationsResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithTags("Stations");
}
