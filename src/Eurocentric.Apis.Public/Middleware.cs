using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Eurocentric.Apis.Public;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the versioned API endpoints defined in the
    ///     <see cref="Eurocentric.Apis.Public" /> assembly.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UsePublicApiEndpoints(this WebApplication app)
    {
        app.MapGet("public/api/v0.1/ping", () => TypedResults.Ok("Public API v0.1 zapped to the extreme!"))
            .WithTags("Placeholders");

        app.MapGet("public/api/v0.2/ping", () => TypedResults.Ok("Public API v0.2 zapped to the extreme!"))
            .WithTags("Placeholders");
    }
}
