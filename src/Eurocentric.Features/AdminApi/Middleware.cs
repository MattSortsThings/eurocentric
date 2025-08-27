using Eurocentric.Features.AdminApi.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the versioned Admin API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    internal static void MapAdminApiEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.NewVersionedApi("AdminApi")
            .MapGroup("admin/api");

        api.MapV1Endpoints();
    }
}
