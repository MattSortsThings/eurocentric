using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.AppPipeline;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Adds the <see cref="MediatR" /> app pipeline services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddAppPipeline(this IServiceCollection services)
    {
        services.AddMediatR(BuildConfiguration(services));

        return services;
    }

    private static MediatRServiceConfiguration BuildConfiguration(IServiceCollection services)
    {
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();

        MediatRServiceConfiguration config = new();

        foreach (IAppPipelineConfigurator configurator in
                 scope.ServiceProvider.GetRequiredService<IEnumerable<IAppPipelineConfigurator>>())
        {
            configurator.Modify(config);
        }

        return config;
    }
}
