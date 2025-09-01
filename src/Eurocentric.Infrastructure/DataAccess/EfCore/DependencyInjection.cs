using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.DataAccess.EfCore;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    internal static IServiceCollection AddEfCoreDataAccess(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            IConfiguration config = serviceProvider.GetRequiredService<IConfiguration>();

            options.UseAzureSql(config.GetConnectionString("AzureSql"), azureSqlOptions =>
                {
                    azureSqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    azureSqlOptions.MigrationsHistoryTable("ef_migrations_history");
                    azureSqlOptions.EnableRetryOnFailure(6, TimeSpan.FromSeconds(30), null);
                    azureSqlOptions.CommandTimeout(30);
                }).UseExceptionProcessor()
                .UseSnakeCaseNamingConvention();
        });

        return services;
    }
}
