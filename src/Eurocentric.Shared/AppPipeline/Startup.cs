using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.AppPipeline;

/// <summary>
///     Extension methods to be invoked on application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the app pipeline services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddAppPipeline(this IServiceCollection services) =>
        services.AddMediatR(CreateConfiguration(services));

    private static MediatRServiceConfiguration CreateConfiguration(IServiceCollection services)
    {
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();

        MediatRServiceConfiguration configuration = new();

        foreach (IAppPipelineConfigurator configurator in scope.ServiceProvider
                     .GetRequiredService<IEnumerable<IAppPipelineConfigurator>>())
        {
            configurator.Configure(configuration);
        }

        return configuration;
    }
}
