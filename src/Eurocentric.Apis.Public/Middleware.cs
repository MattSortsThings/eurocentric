using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the versioned Public API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UsePublicApiVersionedEndpoints(this WebApplication app)
    {
        RouteGroupBuilder apiGroup = app.MapGroup("public/api").AllowAnonymous();

        apiGroup.MapGet("placeholder", () => TypedResults.Ok("Public API zapped to the extreme!"));
    }
}
