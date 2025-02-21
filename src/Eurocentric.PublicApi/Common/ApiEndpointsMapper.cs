using Eurocentric.PublicApi.V0.Stations.GetStations;
using Eurocentric.Shared.ApiMapping;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi.Common;

internal sealed class ApiEndpointsMapper : IApiEndpointsMapper
{
    public void Map(IEndpointRouteBuilder app) => app.MapGetStationsV0Point1();
}
