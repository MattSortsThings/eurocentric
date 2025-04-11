using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Eurocentric.Features.Shared.ApiRegistration;

internal static class DependencyInjection
{
    internal static IServiceCollection DiscoverApis(this IServiceCollection services)
    {
        ServiceDescriptor[] apiDescriptors = typeof(DependencyInjection).Assembly.DefinedTypes
            .Where(type => typeof(IApiInfo).IsAssignableFrom(type) && type is { IsAbstract: false, IsInterface: false })
            .Select(type => new ServiceDescriptor(typeof(IApiInfo), type, ServiceLifetime.Transient))
            .ToArray();

        ServiceDescriptor[] endpointDescriptors = typeof(DependencyInjection).Assembly.DefinedTypes
            .Where(type => typeof(IEndpointInfo).IsAssignableFrom(type) && type is { IsAbstract: false, IsInterface: false })
            .Select(type => new ServiceDescriptor(typeof(IEndpointInfo), type, ServiceLifetime.Transient))
            .ToArray();

        services.TryAddEnumerable(apiDescriptors);
        services.TryAddEnumerable(endpointDescriptors);

        return services;
    }
}
