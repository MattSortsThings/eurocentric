using Eurocentric.Apis.Public.V0;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Maps the Public API endpoints to the endpoint route builder.
    /// </summary>
    /// <param name="builder">The web application endpoint route builder.</param>
    public static void MapPublicApiEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder apiGroup = builder.MapGroup("/public/api");

        apiGroup.MapV0Endpoints();
    }
}
