using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.DataAccess.EfCore;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the application database context to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="connectionString">The database connection string.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is <see langword="null" />.</exception>
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, string? connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddDbContext<AppDbContext>(options =>
            options.UseAzureSql(connectionString, ConfigureDbContextOptions)
                .UseEnumCheckConstraints()
                .UseSnakeCaseNamingConvention()
                .UseExceptionProcessor());

        return services;
    }

    private static void ConfigureDbContextOptions(AzureSqlDbContextOptionsBuilder options)
    {
        options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        options.MigrationsHistoryTable("ef_core_migration");
    }
}
