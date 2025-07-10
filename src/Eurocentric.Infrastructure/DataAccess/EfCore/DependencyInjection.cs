using EntityFramework.Exceptions.SqlServer;
using Eurocentric.Infrastructure.DataAccess.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.DataAccess.EfCore;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the Entity Framework Core data access services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="configuration">Contains configuration properties for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddEfCoreDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseAzureSql(configuration.GetConnectionString(DbConstants.ConnectionStringKey),
                    azureSqlOptions =>
                    {
                        azureSqlOptions.MigrationsHistoryTable("ef_migrations_history");
                        azureSqlOptions.EnableRetryOnFailure(configuration.GetValue<int>("DbConnection:MaxRetryCount"));
                        azureSqlOptions.CommandTimeout(configuration.GetValue<int>("DbConnection:CommandTimeoutInSeconds"));
                    })
                .UseSnakeCaseNamingConvention()
                .UseExceptionProcessor();
        });

        return services;
    }
}
