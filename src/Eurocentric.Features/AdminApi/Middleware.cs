using Eurocentric.Features.AdminApi.V0;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the Admin API endpoints to the endpoint route builder.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    internal static void MapAdminApiEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder apiGroup = builder.NewVersionedApi("AdminApi")
            .MapGroup("admin/api");

        apiGroup.MapV0Endpoints();
    }
}
