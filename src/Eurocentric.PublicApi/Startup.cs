using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi;

/// <summary>
///     Extension methods to be invoked on application startup.
/// </summary>
public static class Startup
{
    public static void MapPublicApiPlaceholderEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("public/api/v0.1/placeholder",
            () => TypedResults.Ok($"Public API zapped to the extreme at {DateTime.Now}."));
}
