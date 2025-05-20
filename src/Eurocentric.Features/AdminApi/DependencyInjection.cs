using Eurocentric.Features.AdminApi.V1.Common.Documentation;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AdminApi;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Registers the OpenAPI documents for the Admin API with the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection RegisterAdminApiOpenApiDocuments(this IServiceCollection services)
    {
        services.RegisterAdminApiV1OpenApiDocuments();

        return services;
    }
}
