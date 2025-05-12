using Eurocentric.Features.PublicApi.V0;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.PublicApi;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Registers the OpenAPI documents for the Public API with the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection RegisterPublicApiOpenApiDocuments(this IServiceCollection services)
    {
        services.RegisterPublicApiV0OpenApiDocuments();

        return services;
    }
}
