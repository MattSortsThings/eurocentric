using Eurocentric.Features.AdminApi.V0;
using Eurocentric.Features.AdminApi.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the Admin API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    internal static void MapAdminApiEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi("AdminApi")
            .MapGroup("admin/api");

        apiGroup.MapAdminApiV0Endpoints();
        apiGroup.MapAdminApiV1Endpoints();
    }
}
