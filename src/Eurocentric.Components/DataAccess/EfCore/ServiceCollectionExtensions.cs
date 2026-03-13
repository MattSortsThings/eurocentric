using EntityFramework.Exceptions.SqlServer;
using Eurocentric.Components.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.DataAccess.EfCore;

/// <summary>
///     Extension methods for the <see cref="IServiceCollection" /> interface.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the Entity Framework Core data access services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The original <see cref="IServiceCollection" /> instance.</returns>
    public static IServiceCollection AddEfCoreDataAccess(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(ConfigureDbContextOptions);

        return services;
    }

    private static void ConfigureDbContextOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder builder)
    {
        (string connectionString, int commandTimeoutInSeconds, int maxRetries) = serviceProvider
            .GetRequiredService<IOptions<DbConnectionOptions>>()
            .Value;

        builder
            .UseAzureSql(
                connectionString,
                azureSqlOptions =>
                    azureSqlOptions
                        .EnableRetryOnFailure(maxRetries)
                        .CommandTimeout(commandTimeoutInSeconds)
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            )
            .UseEnumCheckConstraints()
            .UseExceptionProcessor();
    }
}
