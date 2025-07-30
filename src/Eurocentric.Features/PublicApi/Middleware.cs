using Eurocentric.Features.PublicApi.V0.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the versioned Public API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    internal static void MapPublicApiEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.NewVersionedApi("PublicApi")
            .MapGroup("public/api");

        api.MapV0Endpoints();
    }
}
