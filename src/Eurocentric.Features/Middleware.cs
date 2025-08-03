using Eurocentric.Features.AdminApi;
using Eurocentric.Features.PublicApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Scalar.AspNetCore;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
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
        app.MapAdminApiEndpoints();
        app.MapPublicApiEndpoints();
    }

    /// <summary>
    ///     Configures the web application to use the anonymous API documentation endpoints defined in the
    ///     <see cref="Eurocentric.Features" /> assembly.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseApiDocumentationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapOpenApi().AllowAnonymous();

        app.MapScalarApiReference("docs").AllowAnonymous();
    }
}
