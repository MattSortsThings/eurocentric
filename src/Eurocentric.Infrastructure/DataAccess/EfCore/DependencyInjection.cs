using EntityFramework.Exceptions.SqlServer;
using Eurocentric.Infrastructure.DataAccess.Common;
using Eurocentric.Infrastructure.DataAccess.EfCore.Interceptors;
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
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddEfCoreDataAccess(this IServiceCollection services)
    {
        services.AddScoped<PublishDomainEventsSaveChangesInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            IConfiguration config = sp.GetRequiredService<IConfiguration>();

            options.UseAzureSql(config.GetConnectionString(DbConfigKeys.ConnectionStrings.AzureSql),
                    azureSqlOptions =>
                    {
                        azureSqlOptions.MigrationsHistoryTable("ef_migrations_history");
                        azureSqlOptions.EnableRetryOnFailure(
                            config.GetValue<int>(DbConfigKeys.DbConnection.MaxRetryCount));
                        azureSqlOptions.CommandTimeout(
                            config.GetValue<int>(DbConfigKeys.DbConnection.CommandTimeoutInSeconds));
                    })
                .UseSnakeCaseNamingConvention()
                .UseExceptionProcessor()
                .AddInterceptors(sp.GetRequiredService<PublishDomainEventsSaveChangesInterceptor>());
        });

        return services;
    }
}
