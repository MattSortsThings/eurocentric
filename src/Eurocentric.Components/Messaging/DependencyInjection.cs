using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SlimMessageBus.Host;
using SlimMessageBus.Host.Memory;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the messaging services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="assemblies">The assemblies to be scanned for messaging service implementations.</param>
    /// <returns>The original <see cref="IServiceCollection" /> instance.</returns>
    public static IServiceCollection AddMessaging(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddSlimMessageBus(builder =>
            builder.WithProviderMemory().AutoDeclareFrom(assemblies, messageTypeToTopicConverter: type => type.FullName)
        );

        return services;
    }
}
