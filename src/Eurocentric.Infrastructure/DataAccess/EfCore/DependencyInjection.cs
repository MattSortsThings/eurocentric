using Eurocentric.Infrastructure.DataAccess.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eurocentric.Infrastructure.DataAccess.EfCore;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the EF Core data access services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddEfCoreDataAccess(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(
            (serviceProvider, dbOptionsBuilder) =>
            {
                string? connString = serviceProvider
                    .GetRequiredService<IConfiguration>()
                    .GetConnectionString(DbConnectionStringKeys.AzureSql);

                Action<AzureSqlDbContextOptionsBuilder> azureConfig = CreateAzureConfig(
                    serviceProvider
                );

                dbOptionsBuilder
                    .UseAzureSql(connString, azureConfig)
                    .UseSnakeCaseNamingConvention();
            }
        );

        return services;
    }

    private static Action<AzureSqlDbContextOptionsBuilder> CreateAzureConfig(
        IServiceProvider serviceProvider
    )
    {
        IOptionsSnapshot<AzureSqlDbOptions> options = serviceProvider.GetRequiredService<
            IOptionsSnapshot<AzureSqlDbOptions>
        >();

        return azureOptionsBuilder =>
        {
            azureOptionsBuilder
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                .EnableRetryOnFailure(options.Value.MaxRetryCount)
                .CommandTimeout(options.Value.CommandTimeoutInSeconds);
        };
    }
}
