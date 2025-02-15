using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the shared services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddMediatR(BuildConfiguration(services));

        return services;
    }

    private static MediatRServiceConfiguration BuildConfiguration(IServiceCollection services)
    {
        MediatRServiceConfiguration config = new();

        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();

        foreach (Action<MediatRServiceConfiguration> action in scope.ServiceProvider
                     .GetRequiredService<IEnumerable<Action<MediatRServiceConfiguration>>>())
        {
            action.Invoke(config);
        }

        return config;
    }
}
