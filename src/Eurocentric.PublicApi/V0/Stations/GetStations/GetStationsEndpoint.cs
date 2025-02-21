using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi.V0.Stations.GetStations;

internal static class GetStationsEndpoint
{
    internal static void MapGetStationsV0Point1(this IEndpointRouteBuilder app) => app.MapGet("public/api/v0.1/stations",
        async ([AsParameters] GetStationsQuery query, ISender sender, CancellationToken cancellationToken = default) =>
        {
            ErrorOr<GetStationsResult> errorsOrResult = await sender.Send(query, cancellationToken);

            return TypedResults.Ok(errorsOrResult.Value);
        });
}
