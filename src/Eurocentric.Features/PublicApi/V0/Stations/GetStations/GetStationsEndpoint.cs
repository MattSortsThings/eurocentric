using ErrorOr;
using Eurocentric.Features.PublicApi.Common;
using Eurocentric.Features.Shared.ApiDiscovery;
using Eurocentric.Features.Shared.ErrorHandling;
using Microsoft.AspNetCore.Http;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.Stations.GetStations;

internal sealed record GetStationsEndpoint : IEndpointInfo
{
    public string Name => nameof(GetStationsEndpoint);

    public HttpMethod HttpMethod => HttpMethod.Get;

    public string Route => "stations";

    public Delegate Handler => static async ([AsParameters] GetStationsQuery request,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await ErrorOrFactory.From(request)
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToResultOrProblemAsync(TypedResults.Ok);

    public string Summary => "Get stations";

    public string Description => "Retrieves a list of all stations matching the query string parameters";

    public string Tag => PublicApiInfo.Tags.Stations;

    public IEnumerable<int> ProblemStatusCodes
    {
        get
        {
            yield return StatusCodes.Status400BadRequest;
        }
    }

    public string ApiName => PublicApiInfo.ApiName;

    public int MajorApiVersion => 0;

    public int MinorApiVersion => 1;
}
