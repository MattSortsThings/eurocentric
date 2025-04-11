using Eurocentric.Features.Shared.Documentation;
using Eurocentric.Features.Shared.EndpointMapping;
using Microsoft.AspNetCore.Builder;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the HTTP request pipeline middleware for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The same <see cref="WebApplication" /> instance, so that method invocations can be chained.</returns>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseStatusCodePages();

        app.UseExceptionHandler();

        app.UseVersionedApiEndpoints();

        app.UseDocumentationEndpoints();

        return app;
    }
}
