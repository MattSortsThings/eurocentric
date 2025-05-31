using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to invoked at application startup.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web app to use the API endpoints defined in the <see cref="Eurocentric.Features" /> assembly.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("admin/api/v0.1/placeholder", () => TypedResults.Ok($"Admin API v0 zapped to the extreme at {DateTime.Now}!"))
            .AllowAnonymous();

        app.MapGet("public/api/v0.1/placeholder",
                () => TypedResults.Ok($"Public API v0 zapped to the extreme at {DateTime.Now}!"))
            .AllowAnonymous();
    }
}
