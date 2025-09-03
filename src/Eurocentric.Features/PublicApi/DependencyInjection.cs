using Eurocentric.Features.PublicApi.V0;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.PublicApi;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the OpenAPI documents for Public API v0.x releases to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddPublicApiOpenApiDocuments(this IServiceCollection services)
    {
        services.AddV0OpenApiDocuments();

        return services;
    }
}
