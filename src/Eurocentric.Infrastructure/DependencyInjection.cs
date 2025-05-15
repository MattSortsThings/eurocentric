using Eurocentric.Infrastructure.EfCore;
using Eurocentric.Infrastructure.FakeRepositories;
using Eurocentric.Infrastructure.IdProviders;
using Eurocentric.Infrastructure.Timing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds all the services from the <see cref="Eurocentric.Infrastructure" /> assembly to the application service
    ///     descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="configuration">Contains configuration properties for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEfCore(configuration)
            .AddFakeDataRepositories()
            .AddIdProviders()
            .AddTiming();

        return services;
    }
}
