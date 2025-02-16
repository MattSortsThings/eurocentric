using Eurocentric.AdminApi;
using Eurocentric.DataAccess;
using Eurocentric.PublicApi;
using Eurocentric.Shared;
using Scalar.AspNetCore;

namespace Eurocentric.WebApp;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Configures the services for the web application to be built.
    /// </summary>
    /// <param name="builder">The web application builder to be configured.</param>
    /// <returns>The same <see cref="WebApplicationBuilder" /> instance, so that method invocations can be chained.</returns>
    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAdminApiServices()
            .AddPublicApiServices()
            .AddDataAccessServices()
            .AddSharedServices();

        return builder;
    }

    /// <summary>
    ///     Configures the request pipeline middleware for the web application.
    /// </summary>
    /// <param name="app">The web application to be configured.</param>
    /// <returns>The same <see cref="WebApplication" /> instance, so that method invocations can be chained.</returns>
    internal static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseStatusCodePages();

        app.UseExceptionHandler();

        app.MapOpenApi();

        app.MapScalarApiReference("docs", options => options.Theme = ScalarTheme.Kepler);

        app.UseApiEndpoints();

        return app;
    }
}
