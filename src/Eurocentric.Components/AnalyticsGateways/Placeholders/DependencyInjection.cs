using Eurocentric.Domain.Placeholders.Analytics.Queryables;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.AnalyticsGateways.Placeholders;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the placeholder analytics gateway services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The original <see cref="IServiceCollection" /> instance.</returns>
    internal static IServiceCollection AddPlaceholderAnalyticsGateways(this IServiceCollection services)
    {
        services.AddScoped<IQueryablesGateway, QueryablesGateway>();

        return services;
    }
}
