using Eurocentric.AdminApi.V0;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi;

/// <summary>
///     Extension methods to be invoked at application composition root.
/// </summary>
public static class Configuration
{
    /// <summary>
    ///     Maps the Admin API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The same web application instance, so that method invocations can be chained.</returns>
    public static IEndpointRouteBuilder MapAdminApiEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder adminApiGroup = app.NewVersionedApi("AdminApi")
            .MapGroup("admin/api");

        adminApiGroup.MapVersion0Point1Release();

        return app;
    }
}
