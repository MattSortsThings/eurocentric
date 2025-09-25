using Eurocentric.Apis.Public.V0.Features.Queryables;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public.V0;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Maps the version 0.x endpoints to the endpoint route builder.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    public static void MapV0Endpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGetQueryableBroadcastsV0Point1()
            .MapGetQueryableContestsV0Point1()
            .MapGetQueryableCountriesV0Point1();
    }
}
