using Eurocentric.Features.AdminApi;
using Eurocentric.Features.PublicApi;
using Microsoft.AspNetCore.Builder;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to be invoked on application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Configures the HTTP request pipeline middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The same <see cref="WebApplication" /> instance, so that method invocations can be chained.</returns>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.MapAdminApiEndpoints();

        app.MapPublicApiEndpoints();

        return app;
    }
}
