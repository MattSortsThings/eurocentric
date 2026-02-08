using Eurocentric.Components.AnalyticsGateways.Placeholders;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.AnalyticsGateways;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the analytics gateway services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The original <see cref="IServiceCollection" /> instance.</returns>
    public static IServiceCollection AddAnalyticsGateways(this IServiceCollection services)
    {
        services.AddPlaceholderAnalyticsGateways();

        return services;
    }
}
