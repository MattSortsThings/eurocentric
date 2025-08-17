using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsAverageRankings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V1.Rankings;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the endpoints tagged with "Rankings".
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapRankingsEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("rankings")
            .WithTags(EndpointNames.Tags.Rankings)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("competing-countries/points-average", GetCompetingCountryPointsAverageRankingsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Rankings.GetCompetingCountryPointsAverageRankings)
            .WithSummary("Get competing country points average rankings")
            .WithDescription("Ranks each competing country by descending POINTS AVERAGE metric, " +
                             "i.e. the average value of all the individual points awards it has received. " +
                             "Returns a page of rankings.")
            .HasApiVersion(1, 0)
            .Produces<GetCompetingCountryPointsAverageRankingsResponse>();
    }
}
