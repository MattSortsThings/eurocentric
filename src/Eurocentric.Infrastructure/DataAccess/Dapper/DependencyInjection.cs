using Dapper;
using Eurocentric.Infrastructure.DataAccess.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.DataAccess.Dapper;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the Dapper data access services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="configuration">Contains configuration properties for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddDapperDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddScoped<IDbConnectionFactory>(_ =>
            new AzureSqlDbConnectionFactory(configuration.GetConnectionString(DbConstants.ConnectionStringKey)!));

        return services;
    }
}
