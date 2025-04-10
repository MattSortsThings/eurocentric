using Eurocentric.Domain.Placeholders;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.Stations.GetStations;

public sealed record GetStationsQuery : Request<GetStationsResponse>
{
    [FromQuery(Name = "line")]
    public required Line Line { get; init; }
}
