using EntityFramework.Exceptions.SqlServer;
using Eurocentric.Infrastructure.EFCore;
using Eurocentric.Infrastructure.InMemoryRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds all the <see cref="Eurocentric.Infrastructure" /> assembly services to the application service descriptor
    ///     collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="configuration">Contains configuration properties for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEfCoreAppDbContext(configuration.GetConnectionString(DbConstants.ConnectionStringKey))
            .AddInMemoryRepositories();

        return services;
    }

    /// <summary>
    ///     Adds the application database context class as a scoped service to the application service descriptor collection,
    ///     using the provided database connection string.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="connectionString">The database connection string.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is <see langword="null" />.</exception>
    public static IServiceCollection AddEfCoreAppDbContext(this IServiceCollection services, string? connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddDbContext<AppDbContext>(options =>
            options.UseAzureSql(connectionString, azureSqlOptions =>
                    azureSqlOptions.MigrationsHistoryTable(DbConstants.TableNames.EfCoreMigration,
                        DbConstants.SchemaName)
                ).UseSnakeCaseNamingConvention()
                .UseExceptionProcessor());

        return services;
    }
}
