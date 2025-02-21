using Eurocentric.PublicApi.V0.Stations.Models;
using Eurocentric.Shared.AppPipeline;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.PublicApi.V0.Stations.GetStations;

public sealed record GetStationsQuery : Query<GetStationsResult>
{
    [FromQuery(Name = "line")]
    public required Line Line { get; init; }
}
