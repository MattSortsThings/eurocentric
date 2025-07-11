using Eurocentric.Features.AdminApi.V0;
using Eurocentric.Features.AdminApi.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi;

/// <summary>
///     Extension methods to invoked when configuring the web application HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps all the versioned Admin API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    internal static void MapAdminApiEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi("admin-api")
            .MapGroup("admin/api");

        apiGroup.MapAdminApiV0Endpoints();
        apiGroup.MapAdminApiV1Endpoints();
    }
}
