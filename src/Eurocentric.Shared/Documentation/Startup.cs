using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.Documentation;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the OpenAPI documents and documentation services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();

        foreach (IOpenApiGenerator generator in scope.ServiceProvider.GetRequiredService<IEnumerable<IOpenApiGenerator>>())
        {
            generator.AddOpenApiDocuments(services);
        }

        return services;
    }

    /// <summary>
    ///     Maps all the documentation endpoints that have been configured for the web application.
    /// </summary>
    /// <remarks>Documentation endpoints allow anonymous access with no authentication.</remarks>
    /// <param name="app">The web application.</param>
    public static void UseDocumentationEndpoints(this IEndpointRouteBuilder app) => app.MapOpenApi().AllowAnonymous();
}
