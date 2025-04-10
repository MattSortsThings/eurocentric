using Eurocentric.Features.AdminApi.Stations.CreateStation;
using Eurocentric.Features.AdminApi.Stations.GetStation;
using Eurocentric.Features.PublicApi.Stations.GetStations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the HTTP request pipeline middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The same <see cref="WebApplication" /> instance, so that method invocations can be chained.</returns>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        RouteGroupBuilder adminApi = app.MapGroup("admin/api").WithGroupName("AdminApi").AllowAnonymous();

        adminApi.MapGetStation();
        adminApi.MapCreateStation();

        RouteGroupBuilder publicApi = app.MapGroup("public/api").WithGroupName("PublicApi").AllowAnonymous();

        publicApi.MapGetStations();

        return app;
    }
}
