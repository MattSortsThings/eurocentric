using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Maps the Admin API endpoints to the endpoint route builder.
    /// </summary>
    /// <param name="builder">The web application endpoint route builder.</param>
    public static void MapAdminApiEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder apiGroup = builder.MapGroup("/admin/api");

        apiGroup
            .MapGet("placeholders", () => TypedResults.Ok("Admin API zapped to the extreme!"))
            .AllowAnonymous();
    }
}
