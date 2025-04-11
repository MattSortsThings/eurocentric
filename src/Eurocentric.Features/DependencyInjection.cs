using Eurocentric.Features.Shared.ApiDiscovery;
using Eurocentric.Features.Shared.Documentation;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Json;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Features.Shared.Security;
using Eurocentric.Features.Shared.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds all the features-level services to the service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        services.DiscoverApis()
            .AddDocumentation()
            .AddErrorHandling()
            .AddMessaging()
            .AddSecurity()
            .AddVersioning()
            .ConfigureOptions<ConfigureJsonOptions>();

        return services;
    }
}
