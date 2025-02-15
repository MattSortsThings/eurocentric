using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.AppPipeline;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Adds the <see cref="MediatR" /> application pipeline services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddAppPipeline(this IServiceCollection services)
    {
        services.AddMediatR(services.BuildConfigurationAction());

        return services;
    }

    private static Action<MediatRServiceConfiguration> BuildConfigurationAction(this IServiceCollection services)
    {
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();

        return scope.ServiceProvider.GetRequiredService<IEnumerable<Action<MediatRServiceConfiguration>>>()
            .Aggregate((MediatRServiceConfiguration _) => { },
                (combinedAction, action) => combinedAction + action);
    }
}
