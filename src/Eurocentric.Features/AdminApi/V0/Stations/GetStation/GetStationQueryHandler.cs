using ErrorOr;
using Eurocentric.Domain.Placeholders;
using Eurocentric.Features.AdminApi.V0.Stations.Models;
using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.V0.Stations.GetStation;

internal sealed class GetStationQueryHandler : RequestHandler<GetStationQuery, GetStationResponse>
{
    public override Task<ErrorOr<GetStationResponse>> OnHandle(GetStationQuery request,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<GetStationResponse> result = request.StationId == 0
            ? Error.NotFound("Station not found",
                "No station exists with the specified ID.",
                new Dictionary<string, object> { ["stationId"] = 0 })
            : ErrorOrFactory.From(new GetStationResponse(new Station(request.StationId, "Mornington Crescent", Line.Northern)));

        return Task.FromResult(result);
    }
}
