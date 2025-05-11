using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the API endpoints that have been defined.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("admin/api/v0.1/placeholder", () =>
                TypedResults.Ok($"Admin API zapped to the extreme at {DateTime.Now}!"))
            .AllowAnonymous();

        app.MapGet("public/api/v0.1/placeholder", () =>
                TypedResults.Ok($"Public API zapped to the extreme at {DateTime.Now}!"))
            .AllowAnonymous();
    }
}
