using EntityFramework.Exceptions.SqlServer;
using Eurocentric.Infrastructure.DataAccess.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.DataAccess.EfCore;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds EF Core data access to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="config">Contains configuration properties for the application.</param>
    internal static void AddEfCoreDataAccess(this IServiceCollection services, IConfiguration config) =>
        services.AddDbContext<AppDbContext>(options =>
            options.UseAzureSql(config.GetConnectionString(DbConfigKeys.ConnectionString), ConfigureAzureSql(config))
                .UseSnakeCaseNamingConvention()
                .UseExceptionProcessor());

    private static Action<AzureSqlDbContextOptionsBuilder> ConfigureAzureSql(IConfiguration config) =>
        azureOptions => azureOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            .MigrationsHistoryTable(DboSchema.Tables.EfMigrationsHistory, DboSchema.Name)
            .EnableRetryOnFailure(config.GetValue<int>(DbConfigKeys.MaxRetryCount))
            .CommandTimeout(config.GetValue<int>(DbConfigKeys.CommandTimeoutInSeconds));
}
