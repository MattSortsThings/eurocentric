using EntityFramework.Exceptions.SqlServer;
using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Components.DataAccess.EFCore;
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
    ///     Adds the EF Core data access services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The original <see cref="IServiceCollection" /> instance.</returns>
    internal static IServiceCollection AddEfCoreDataAccess(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(ConfigureDbContextOptions);

        return services;
    }

    private static void ConfigureDbContextOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder builder)
    {
        AzureSqlDbOptions azureSqlOptionsValue = serviceProvider
            .GetRequiredService<IOptions<AzureSqlDbOptions>>()
            .Value;

        builder
            .UseAzureSql(
                azureSqlOptionsValue.ConnectionString,
                azureOptions =>
                {
                    azureOptions
                        .CommandTimeout(azureSqlOptionsValue.CommandTimeoutInSeconds)
                        .EnableRetryOnFailure(azureSqlOptionsValue.MaxRetries)
                        .MigrationsHistoryTable(DbTables.Dbo.EfMigrationsHistory, DbSchemas.Dbo)
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }
            )
            .UseEnumCheckConstraints()
            .UseExceptionProcessor();
    }
}
