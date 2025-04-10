using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the HTTP request pipeline middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The same <see cref="WebApplication" /> instance, so that method invocations can be chained.</returns>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.MapGet("admin/api/v0.1/placeholder", () => TypedResults.Ok($"Admin API zapped to the extreme at {DateTime.Now}!"));
        app.MapGet("public/api/v0.1/placeholder", () => TypedResults.Ok($"Public API zapped to the extreme at {DateTime.Now}!"));

        return app;
    }
}
