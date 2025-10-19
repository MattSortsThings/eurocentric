using Eurocentric.Components.Gateways.V0;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.Gateways;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the domain query gateway services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddGateways(this IServiceCollection services)
    {
        services.AddV0Gateways();

        return services;
    }
}
