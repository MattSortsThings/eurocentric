using Microsoft.Extensions.DependencyInjection;
using SlimMessageBus.Host;
using SlimMessageBus.Host.Memory;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the application pipeline messaging services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSlimMessageBus(builder => builder.WithProviderMemory()
            .AutoDeclareFrom(typeof(DependencyInjection).Assembly, messageTypeToTopicConverter: type => type.FullName));

        return services;
    }
}
