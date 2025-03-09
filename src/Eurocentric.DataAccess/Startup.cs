using Eurocentric.DataAccess.DbCheckers;
using Eurocentric.DataAccess.EfCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.DataAccess;

/// <summary>
///     Extension methods to be invoked on application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the data access services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="configuration">Contains configuration properties for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbCheckers()
            .AddAppDbContext(configuration.GetConnectionString("Eurocentric"));

        return services;
    }
}
