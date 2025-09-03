using Eurocentric.Features.AdminApi;
using Eurocentric.Features.PublicApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Scalar.AspNetCore;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use all the versioned API endpoints defined in the
    ///     <see cref="Eurocentric.Features" /> assembly.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseVersionedApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAdminApiEndpoints();
        app.MapPublicApiEndpoints();
    }

    /// <summary>
    ///     Configures the web application to use all the OpenAPI documentation endpoints defined in the
    ///     <see cref="Eurocentric.Features" /> assembly.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseDocumentationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapOpenApi().AllowAnonymous();
        app.MapScalarApiReference("docs").AllowAnonymous();
    }
}
