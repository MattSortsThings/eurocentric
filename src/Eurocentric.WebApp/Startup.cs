using Eurocentric.Apis.Admin;
using Eurocentric.Apis.Public;
using Eurocentric.Infrastructure.DataAccess;
using Eurocentric.Infrastructure.HttpJson;
using Eurocentric.Infrastructure.Messaging;
using Middleware = Eurocentric.Apis.Admin.Middleware;

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
    /// <returns>The same <see cref="WebApplicationBuilder" /> instance, so that method invocations can be chained.</returns>
    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder
            .Services.AddDataAccess()
            .AddHttpJsonConfiguration()
            .AddMessaging(typeof(Middleware).Assembly, typeof(Apis.Public.Middleware).Assembly);

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

        app.MapAdminApiEndpoints();
        app.MapPublicApiEndpoints();

        return app;
    }
}
