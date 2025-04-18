using Eurocentric.Features.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the feature services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        services.AddSharedServices();

        return services;
    }
}
