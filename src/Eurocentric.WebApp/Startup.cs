using Eurocentric.AdminApi;
using Eurocentric.DataAccess;
using Eurocentric.PublicApi;
using Eurocentric.Shared.AppPipeline;
using Eurocentric.Shared.Documentation;
using Eurocentric.Shared.Endpoints;
using Eurocentric.Shared.ErrorHandling;
using Eurocentric.Shared.Json;
using Eurocentric.Shared.Security;
using Eurocentric.Shared.Versioning;

namespace Eurocentric.WebApp;

/// <summary>
///     Extension methods to be invoked on application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Configures the services for the web application.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The same <see cref="WebApplicationBuilder" /> instance, so that method invocations can be chained.</returns>
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAdminApi()
            .AddPublicApi()
            .AddAppPipeline()
            .AddDataAccess(builder.Configuration)
            .AddDocumentation()
            .AddErrorHandling()
            .AddJsonConfiguration()
            .AddSecurity()
            .AddVersioning();

        return builder;
    }

    /// <summary>
    ///     Configures the HTTP request pipeline middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The same <see cref="WebApplication" /> instance, so that method invocations can be chained.</returns>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseStatusCodePages();

        app.UseExceptionHandler();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseApiEndpoints();

        app.UseDocumentationEndpoints();

        return app;
    }
}
