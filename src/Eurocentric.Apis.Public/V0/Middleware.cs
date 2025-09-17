using Eurocentric.Apis.Public.V0.Features.Queryables;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public.V0;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the v0.x endpoints to the route builder.
    /// </summary>
    /// <param name="builder">The route builder.</param>
    internal static void MapV0Endpoints(this IEndpointRouteBuilder builder) =>
        builder.MapGetQueryableBroadcastsV0Point1()
            .MapGetQueryableContestsV0Point1()
            .MapGetQueryableCountriesV0Point1();
}
