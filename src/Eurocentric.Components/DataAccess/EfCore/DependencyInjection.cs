using EntityFramework.Exceptions.SqlServer;
using Eurocentric.Components.DataAccess.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.DataAccess.EfCore;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the Entity Framework Core data access services to the service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>
    ///     The original <see cref="IServiceCollection" /> instance, so that method invocations can be chained.
    /// </returns>
    internal static IServiceCollection AddEfCoreDataAccess(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(
            static (serviceProvider, options) =>
            {
                AzureSqlDbOptions azureSqlDbOptions = serviceProvider
                    .GetRequiredService<IOptions<AzureSqlDbOptions>>()
                    .Value;

                options
                    .UseAzureSql(
                        azureSqlDbOptions.ConnectionString,
                        azureBuilder =>
                        {
                            azureBuilder
                                .EnableRetryOnFailure(azureSqlDbOptions.MaxRetries)
                                .CommandTimeout(azureSqlDbOptions.CommandTimeoutInSeconds)
                                .MigrationsHistoryTable("__ef_migrations_history", DbSchemas.Dbo)
                                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        }
                    )
                    .UseEnumCheckConstraints()
                    .UseExceptionProcessor();
            }
        );

        return services;
    }
}
