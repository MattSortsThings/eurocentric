using ErrorOr;
using Eurocentric.Features.AdminApi.Common;
using Eurocentric.Features.Shared.ApiRegistration;
using Eurocentric.Features.Shared.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Stations.GetStation;

internal sealed record GetStationEndpoint : IEndpointInfo
{
    public string Name => nameof(GetStationEndpoint);

    public HttpMethod HttpMethod => HttpMethod.Get;

    public string Route => "stations/{stationId:int}";

    public Delegate Handler => static async ([FromRoute] int stationId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await ErrorOrFactory.From(new GetStationQuery(stationId))
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToResultOrProblemAsync(TypedResults.Ok);

    public string Summary => "Get a station";

    public string Description => "Retrieves a single station. The station ID must be supplied as a route parameter.";

    public string Tag => AdminApiInfo.Tags.Stations;

    public IEnumerable<int> ProblemStatusCodes
    {
        get
        {
            yield return StatusCodes.Status404NotFound;
        }
    }

    public string ApiName => AdminApiInfo.ApiName;

    public int MajorApiVersion => 0;

    public int MinorApiVersion => 1;
}
