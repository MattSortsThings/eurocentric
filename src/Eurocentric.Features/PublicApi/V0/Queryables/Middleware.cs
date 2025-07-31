using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableContestStages;
using Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableCountries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V0.Queryables;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the endpoints tagged with "Queryables".
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapQueryablesEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("queryables")
            .WithTags(EndpointNames.Tags.Queryables);

        group.MapGet("contest-stages", GetQueryableContestStagesFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Queryables.GetQueryableContestStages)
            .WithSummary("Get queryable contest stages")
            .WithDescription("Retrieves a list of all queryable contest stage enum values.")
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
            .Produces<GetQueryableContestStagesResponse>();

        group.MapGet("countries", GetQueryableCountriesFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Queryables.GetQueryableCountries)
            .WithSummary("Get queryable countries")
            .WithDescription("Retrieves a list of all queryable countries, in country code order.")
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
            .Produces<GetQueryableCountriesResponse>();
    }
}
