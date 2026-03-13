using Eurocentric.Components.DataAccess.EfCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.DataAccess;

/// <summary>
///     Extension methods for the <see cref="IServiceCollection" /> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the data access services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The original <see cref="IServiceCollection" /> instance.</returns>
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddEfCoreDataAccess();

        return services;
    }
}
