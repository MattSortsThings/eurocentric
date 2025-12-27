using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.WebApp;

/// <summary>
/// Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    /// Configures the services for the web application to be built.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The original <see cref="WebApplicationBuilder"/> instance.</returns>
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        _ = builder.Services;

        return builder;
    }

    /// <summary>
    /// Configures the HTTP request pipeline middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The original <see cref="WebApplication"/> instance.</returns>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.MapGet("/blobby", () => TypedResults.Ok(Enumerable.Repeat("Blobby!",3).ToArray()))
            .WithName("GetBlobby");

        return app;
    }
}
