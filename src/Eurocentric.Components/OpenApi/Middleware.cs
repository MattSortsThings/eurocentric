using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;

namespace Eurocentric.Components.OpenApi;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use OpenAPI endpoints, which do not authenticate requests.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseOpenApiEndpoints(this WebApplication app)
    {
        app.MapOpenApi().AllowAnonymous();

        app.MapScalarApiReference("docs").AllowAnonymous();
    }
}
