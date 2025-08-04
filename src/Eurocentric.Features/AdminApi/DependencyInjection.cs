using Eurocentric.Features.AdminApi.V0;
using Eurocentric.Features.AdminApi.V1;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AdminApi;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the Admin API OpenAPI documents to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddAdminApiOpenApiDocuments(this IServiceCollection services)
    {
        services.AddV0OpenApiDocuments();
        services.AddV1OpenApiDocuments();

        return services;
    }
}
