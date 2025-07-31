using Dapper;
using Eurocentric.Infrastructure.DataAccess.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.DataAccess.Dapper;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the Dapper data access services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddDapperDataAccess(this IServiceCollection services)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddScoped<IDbStoredProcedureRunner>(sp =>
        {
            IConfiguration config = sp.GetRequiredService<IConfiguration>();

            return DbStoredProcedureRunner.Create(
                config.GetConnectionString(DbConfigKeys.ConnectionStrings.AzureSql),
                config.GetValue<int>(DbConfigKeys.DbConnection.CommandTimeoutInSeconds)
            );
        });

        return services;
    }
}
