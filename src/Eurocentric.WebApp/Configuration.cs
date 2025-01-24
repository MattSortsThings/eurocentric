using Eurocentric.Shared.ApiModules;
using Eurocentric.Shared.OpenApi;

namespace Eurocentric.WebApp;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Configuration
{
    /// <summary>
    ///     Configures the HTTP request pipeline for the web application.
    /// </summary>
    /// <param name="app">The web application to be modified.</param>
    /// <returns>The same <see cref="WebApplication" /> instance, so that method invocations can be chained.</returns>
    public static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
        app.UseExceptionHandler();

        app.MapOpenApi();

        app.UseDocumentationPages();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseVersionedApiEndpoints();

        app.UseStatusCodePages();

        return app;
    }
}
