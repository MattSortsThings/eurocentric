using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Components.DataAccess.EfCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.DataAccess;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the data access services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddEfCoreDataAccess().ConfigureOptions<ConfigureAzureSqlDbOptions>();

        return services;
    }
}
