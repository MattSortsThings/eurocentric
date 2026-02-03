using Eurocentric.Apis.Admin;
using Eurocentric.Apis.Public;
using Eurocentric.Components.DataAccess;
using Eurocentric.Components.Endpoints;
using Eurocentric.Components.HttpJson;
using Eurocentric.Components.Messaging;

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
    /// <returns>The original <see cref="WebApplicationBuilder" /> instance.</returns>
    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder
            .Services.AddDataAccess()
            .AddHttpJsonConfiguration()
            .AddMessaging(typeof(AdminApiEndpoints).Assembly, typeof(PublicApiEndpoints).Assembly);

        return builder;
    }

    /// <summary>
    ///     Configures the HTTP request pipeline middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The original <see cref="WebApplication" /> instance.</returns>
    internal static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.Map<AdminApiEndpoints>().Map<PublicApiEndpoints>();

        return app;
    }
}
