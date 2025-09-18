using Dapper;
using Eurocentric.Infrastructure.DataAccess.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.DataAccess.Dapper;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds Dapper data access to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="config">Contains configuration properties for the application.</param>
    internal static void AddDapperDataAccess(this IServiceCollection services, IConfiguration config)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddScoped<IDbSprocRunner>(_ => DbSprocRunner.Create(
            config.GetConnectionString(DbConfigKeys.ConnectionString),
            config.GetValue<int>(DbConfigKeys.CommandTimeoutInSeconds)));
    }
}
