using Eurocentric.PublicApi.V0;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.PublicApi;

/// <summary>
///     Extension methods to be invoked at application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds all the OpenAPI documents for the Public API to the service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddPublicApiOpenApiDocuments(this IServiceCollection services)
    {
        services.AddVersion0Point1OpenApiDocument();

        return services;
    }
}
