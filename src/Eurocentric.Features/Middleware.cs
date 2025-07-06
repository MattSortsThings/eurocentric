using Eurocentric.Features.AdminApi.V0;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to invoked when configuring the web application HTTP request pipeline middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the versioned API endpoints defined in the
    ///     <see cref="Eurocentric.Features" /> assembly.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseVersionedApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAdminApiV0Endpoints();

        app.MapGet("public/api/v0.1/placeholder",
                () => TypedResults.Ok($"Public API v0.1 zapped to the extreme at {DateTime.Now}."))
            .AllowAnonymous();

        app.MapGet("public/api/v0.2/placeholder",
                () => TypedResults.Ok($"Public API v0.2 zapped to the extreme at {DateTime.Now}."))
            .AllowAnonymous();
    }
}
