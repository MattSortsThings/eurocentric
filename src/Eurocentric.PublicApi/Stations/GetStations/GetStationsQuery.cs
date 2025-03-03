using Eurocentric.PublicApi.Stations.Models;
using Eurocentric.Shared.AppPipeline;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.PublicApi.Stations.GetStations;

public sealed record GetStationsQuery : Query<GetStationsResult>
{
    [FromQuery(Name = "line")]
    public required Line Line { get; init; }
}
