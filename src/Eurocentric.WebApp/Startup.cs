using Eurocentric.Apis.Admin;
using Eurocentric.Apis.Public;
using Eurocentric.Infrastructure.HttpJson;
using Eurocentric.Infrastructure.Messaging;

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
        builder.Services.AddHttpJsonConfiguration()
            .AddMessaging(typeof(IAdminApiAssemblyMarker).Assembly, typeof(IPublicApiAssemblyMarker).Assembly);

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

        app.UseAdminApiEndpoints();
        app.UsePublicApiEndpoints();

        return app;
    }
}
