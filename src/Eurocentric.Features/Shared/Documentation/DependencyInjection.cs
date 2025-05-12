using Eurocentric.Features.AdminApi;
using Eurocentric.Features.PublicApi;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.Shared.Documentation;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the OpenAPI documents and documentation services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services.RegisterAdminApiOpenApiDocuments().RegisterPublicApiOpenApiDocuments();

        return services;
    }
}
