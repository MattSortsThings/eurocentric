using Eurocentric.PublicApi.V0;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi;

/// <summary>
///     Extension methods to be invoked at application composition root.
/// </summary>
public static class Configuration
{
    /// <summary>
    ///     Maps the Public API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The same web application instance, so that method invocations can be chained.</returns>
    public static IEndpointRouteBuilder MapPublicApiEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi("PublicApi")
            .MapGroup("public/api");

        apiGroup.MapVersion0Point1Release();

        return app;
    }
}
