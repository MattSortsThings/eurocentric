using Eurocentric.Apis.Public.V0;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the Public API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UsePublicApiEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("public/api");

        group.MapV0Endpoints();
    }
}
