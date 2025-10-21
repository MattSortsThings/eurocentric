using Eurocentric.Apis.Admin;
using Eurocentric.Apis.Public;
using Eurocentric.Components.DataAccess;
using Eurocentric.Components.ErrorHandling;
using Eurocentric.Components.Gateways;
using Eurocentric.Components.HttpJson;
using Eurocentric.Components.Messaging;
using Eurocentric.Components.Repositories;
using Eurocentric.Components.Versioning;
using AdminApiMiddleware = Eurocentric.Apis.Admin.Middleware;
using PublicApiMiddleware = Eurocentric.Apis.Public.Middleware;

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
    /// <returns>
    ///     The same <see cref="WebApplicationBuilder" /> instance, so that method invocations can be chained.
    /// </returns>
    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder
            .Services.AddDataAccess()
            .AddErrorHandling()
            .AddGateways()
            .AddMessaging(typeof(AdminApiMiddleware).Assembly, typeof(PublicApiMiddleware).Assembly)
            .AddRepositories()
            .AddVersioning()
            .ConfigureHttpJsonOptions();

        return builder;
    }

    /// <summary>
    ///     Configures the HTTP request pipeline middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The same <see cref="WebApplication" /> instance, so that method invocations can be chained.</returns>
    internal static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseStatusCodePages();

        app.UseExceptionHandler();

        app.UseAdminApiVersionedEndpoints();
        app.UsePublicApiVersionedEndpoints();

        return app;
    }
}
