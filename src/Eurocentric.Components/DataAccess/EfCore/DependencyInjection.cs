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
        DbConnectionOptions dbConnectionOptions = serviceProvider
            .GetRequiredService<IOptions<DbConnectionOptions>>()
            .Value;

        builder
            .UseAzureSql(
                dbConnectionOptions.ConnectionString,
                azureOptions =>
                {
                    azureOptions
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                        .CommandTimeout(dbConnectionOptions.CommandTimeoutInSeconds)
                        .EnableRetryOnFailure(dbConnectionOptions.MaxRetries);
                }
            )
            .UseEnumCheckConstraints()
            .UseExceptionProcessor();
    }
}
