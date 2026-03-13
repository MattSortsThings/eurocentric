namespace Eurocentric.WebApp.Startup;

/// <summary>
///     Extension methods for the <see cref="WebApplication" /> class.
/// </summary>
internal static class WebApplicationExtensions
{
    /// <summary>
    ///     Configures the HTTP request pipeline for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The original <see cref="WebApplication" /> instance.</returns>
    public static WebApplication ConfigureHttpRequestPipeline(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.MapGet("blobby", () => TypedResults.Ok("Blobby Blobby Blobby!"))
            .AllowAnonymous();

        return app;
    }
}
