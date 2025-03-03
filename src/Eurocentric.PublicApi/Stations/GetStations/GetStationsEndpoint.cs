using ErrorOr;
using Eurocentric.Shared.ApiAbstractions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Eurocentric.PublicApi.Stations.GetStations;

internal sealed record GetStationsEndpoint : IEndpointInfo
{
    public string Name => nameof(GetStations);

    public Delegate Handler => async ([AsParameters] GetStationsQuery query,
        ISender sender,
        CancellationToken cancellationToken = default) =>
    {
        ErrorOr<GetStationsResult> errorsOrResult = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(errorsOrResult.Value);
    };

    public HttpMethod Method => HttpMethod.Get;

    public string Route => "stations";

    public int MajorApiVersion => 0;

    public int MinorApiVersion => 1;
}
