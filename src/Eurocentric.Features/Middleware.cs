using Eurocentric.Features.AdminApi.V0;
using Eurocentric.Features.PublicApi.V0;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to invoked when configuring the web application HTTP request pipeline middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the versioned API endpoints defined in the
    ///     <see cref="Eurocentric.Features" /> assembly.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseVersionedApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAdminApiV0Endpoints();
        app.MapPublicApiV0Endpoints();
    }
}
