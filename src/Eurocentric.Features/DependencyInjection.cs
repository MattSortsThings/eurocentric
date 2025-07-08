using Eurocentric.Features.AdminApi.V0;
using Eurocentric.Features.PublicApi.V0;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.HttpJson;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Features.Shared.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the <see cref="Eurocentric.Features" /> assembly services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddFeaturesServices(this IServiceCollection services)
    {
        services.AddErrorHandling()
            .AddHttpJsonConfiguration()
            .AddMessaging()
            .AddVersioning();

        services.AddAdminApiV0OpenApiDocuments()
            .AddPublicApiV0OpenApiDocuments();

        return services;
    }
}
