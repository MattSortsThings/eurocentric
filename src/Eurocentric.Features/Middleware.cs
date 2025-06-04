using Eurocentric.Features.AdminApi.V0;
using Eurocentric.Features.PublicApi.V0;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Scalar.AspNetCore;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web app to use the API endpoints defined in the <see cref="Eurocentric.Features" /> assembly.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAdminApiV0Endpoints();
        app.MapPublicApiV0Endpoints();
    }

    /// <summary>
    ///     Configures the web app to use the API documentation endpoints defined in the <see cref="Eurocentric.Features" />
    ///     assembly.
    /// </summary>
    /// <remarks>Documentation endpoints do not perform authentication.</remarks>
    /// <param name="app">The web application.</param>
    public static void UseApiDocumentationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapOpenApi().AllowAnonymous();
        app.MapScalarApiReference(DocumentationConstants.DocumentationEndpointPrefix).AllowAnonymous();
    }
}
