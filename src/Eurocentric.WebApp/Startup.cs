using Eurocentric.Features;
using Eurocentric.Infrastructure;

namespace Eurocentric.WebApp;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Configures all the services for the web application to be built.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The same <see cref="WebApplicationBuilder" /> instance, so that method invocations can be chained.</returns>
    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddFeaturesAssemblyServices()
            .AddInfrastructureAssemblyServices();

        return builder;
    }

    /// <summary>
    ///     Configures all the HTTP request pipeline middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The same <see cref="WebApplication" /> instance, so that method invocations can be chained.</returns>
    internal static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseStatusCodePages();

        app.UseExceptionHandler();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseVersionedApiEndpoints();

        app.UseDocumentationEndpoints();

        return app;
    }
}
