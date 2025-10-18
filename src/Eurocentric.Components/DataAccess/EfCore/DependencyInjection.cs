using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.DataAccess.EfCore;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the EF Core data access services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddEfCoreDataAccess(this IServiceCollection services)
    {
        services.AddSingleton<DbContextOptionsConfigurator>();

        services.AddDbContext<AppDbContext>(
            (serviceProvider, options) =>
                serviceProvider.GetRequiredService<DbContextOptionsConfigurator>().Configure(options)
        );

        return services;
    }
}
