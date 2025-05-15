using EntityFramework.Exceptions.SqlServer;
using Eurocentric.Infrastructure.EfCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.EfCore;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds all the EF Core services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="configuration">Contains configuration properties for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddEfCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppDbContext(configuration.GetConnectionString(DbConstants.ConnectionStringKey));

        return services;
    }

    /// <summary>
    ///     Adds the <see cref="AppDbContext" /> EF Core database context class as a scoped service to the application service
    ///     descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="connectionString">
    ///     The database connection string (typically read from the application configuration properties, or passed in
    ///     manually during containerized tests).
    /// </param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseAzureSql(connectionString, a =>
            {
                a.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                a.MigrationsHistoryTable(DbConstants.TableNames.EfCoreMigration);
            })
            .UseSnakeCaseNamingConvention()
            .UseEnumCheckConstraints()
            .UseExceptionProcessor());

        return services;
    }
}
