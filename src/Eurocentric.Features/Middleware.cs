using Eurocentric.Features.AdminApi;
using Eurocentric.Features.PublicApi;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the API endpoints that have been defined.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAdminApiEndpoints();
        app.MapPublicApiEndpoints();
    }
}
