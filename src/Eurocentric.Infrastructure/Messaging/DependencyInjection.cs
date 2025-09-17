using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SlimMessageBus.Host;
using SlimMessageBus.Host.Memory;

namespace Eurocentric.Infrastructure.Messaging;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the application pipeline messaging services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="assemblies">The assemblies to scan for messaging service implementations.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddMessaging(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddSlimMessageBus(builder => builder.WithProviderMemory()
            .AutoDeclareFrom(assemblies, messageTypeToTopicConverter: type => type.FullName));

        return services;
    }
}
