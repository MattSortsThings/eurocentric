using ErrorOr;
using Eurocentric.Features.AdminApi.Common;
using Eurocentric.Features.AdminApi.V0.Stations.GetStation;
using Eurocentric.Features.Shared.ApiRegistration;
using Eurocentric.Features.Shared.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Stations.CreateStation;

internal sealed record CreateStationEndpoint : IEndpointInfo
{
    public string Name => nameof(CreateStationEndpoint);

    public HttpMethod HttpMethod => HttpMethod.Post;

    public string Route => "stations";

    public Delegate Handler => static async ([FromBody] CreateStationCommand request,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await ErrorOrFactory.From(request)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToResultOrProblemAsync(MapToCreatedAtRoute);

    public string Summary => "Create a station";

    public string Description => "Creates a new station from the request body.";

    public string Tag => AdminApiInfo.Tags.Stations;

    public IEnumerable<int> ProblemStatusCodes
    {
        get
        {
            yield return StatusCodes.Status400BadRequest;
            yield return StatusCodes.Status409Conflict;
            yield return StatusCodes.Status422UnprocessableEntity;
        }
    }

    public string ApiName => AdminApiInfo.ApiName;

    public int MajorApiVersion => 0;

    public int MinorApiVersion => 2;

    private static CreatedAtRoute<CreateStationResponse> MapToCreatedAtRoute(CreateStationResponse response) =>
        TypedResults.CreatedAtRoute(response,
            nameof(GetStationEndpoint),
            new RouteValueDictionary { ["stationId"] = response.Station.Id });
}
