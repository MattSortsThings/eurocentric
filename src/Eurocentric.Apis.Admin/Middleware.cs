using Eurocentric.Apis.Admin.V0.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the Admin API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseAdminApiEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("admin/api");

        group.MapV0Endpoints();
    }
}
