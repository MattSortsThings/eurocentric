using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the Admin API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseAdminApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("admin/api/v0.1/placeholders",
                () => TypedResults.Ok($"Admin API v0.1 zapped to the extreme at {DateTime.UtcNow}!"))
            .AllowAnonymous();

        app.MapGet("admin/api/v0.2/placeholders",
                () => TypedResults.Ok($"Admin API v0.2 zapped to the extreme at {DateTime.UtcNow}!"))
            .AllowAnonymous();
    }
}
