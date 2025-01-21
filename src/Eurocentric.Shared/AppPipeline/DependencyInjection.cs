using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.AppPipeline;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the <see cref="MediatR" /> app pipeline services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">The application service descriptor collection to be modified.</param>
    /// <param name="assemblies">The assemblies to be scanned by <see cref="MediatR" /> during configuration.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddAppPipeline(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assemblies));

        return services;
    }
}
