using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.Mapping;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the <see cref="Mapster" /> mapping services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">The application service descriptor collection to be modified.</param>
    /// <param name="assemblies">The assemblies to be scanned by <see cref="MediatR" /> during configuration.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddMapping(this IServiceCollection services, params Assembly[] assemblies)
    {
        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assemblies);

        services.AddSingleton(config);

        return services;
    }
}
