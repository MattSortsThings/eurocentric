using Eurocentric.Features.PublicApi.V0.CompetitorRankings.GetPointsInRangeCompetitorRankings;
using Eurocentric.Features.PublicApi.V0.QueryableCountries.GetQueryableCountries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi;

internal static class EndpointMapping
{
    internal static void MapPublicApiEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.MapGroup("public/api").AllowAnonymous();

        apiGroup.MapGetQueryableCountriesV0Point1();
        apiGroup.MapGetPointsInRangeCompetitorRankingsV0Point1();
    }
}
