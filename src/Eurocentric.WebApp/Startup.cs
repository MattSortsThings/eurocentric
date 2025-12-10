using Eurocentric.Apis.Admin;
using Eurocentric.Apis.Public;
using Eurocentric.Components.DataAccess;

namespace Eurocentric.WebApp;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Configures the services for the web application to be built.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The original <see cref="WebApplicationBuilder" /> instance, so that method invocations should be chained.</returns>
    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDataAccess();

        return builder;
    }

    /// <summary>
    ///     Configures the middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The original <see cref="WebApplication" /> instance, so that method invocations can be chained.</returns>
    internal static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseAdminApiEndpoints();
        app.UsePublicApiEndpoints();

        return app;
    }
}
