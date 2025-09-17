using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the Public API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UsePublicApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("public/api/v0.1/placeholders",
                () => TypedResults.Ok($"Public API v0.1 zapped to the extreme at {DateTime.UtcNow}!"))
            .AllowAnonymous();

        app.MapGet("public/api/v0.2/placeholders",
                () => TypedResults.Ok($"Public API v0.2 zapped to the extreme at {DateTime.UtcNow}!"))
            .AllowAnonymous();
    }
}
