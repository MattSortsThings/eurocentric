using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi;

/// <summary>
///     Extension methods to be invoked on application startup.
/// </summary>
public static class Startup
{
    public static void MapAdminApiPlaceholderEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("admin/api/v0.1/placeholder",
            () => TypedResults.Ok($"Admin API zapped to the extreme at {DateTime.Now}."));
}
