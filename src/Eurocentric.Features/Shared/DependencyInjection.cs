using Eurocentric.Features.Shared.Json;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.Shared;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the shared services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddHttpJsonOptionsConfiguration()
            .AddMessaging();

        return services;
    }
}
