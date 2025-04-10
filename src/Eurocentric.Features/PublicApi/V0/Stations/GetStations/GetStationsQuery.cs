using Eurocentric.Domain.Placeholders;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V0.Stations.GetStations;

public sealed record GetStationsQuery : Request<GetStationsResponse>
{
    [FromQuery(Name = "line")]
    public required Line Line { get; init; }
}
