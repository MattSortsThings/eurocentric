using EntityFramework.Exceptions.SqlServer;
using Eurocentric.Components.DataAccess.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.DataAccess.EfCore;

internal sealed class DbContextOptionsConfigurator(IOptions<AzureSqlDbOptions> options)
{
    internal void Configure(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseAzureSql(
                options.Value.ConnectionString,
                builder =>
                {
                    builder
                        .CommandTimeout(options.Value.CommandTimeoutInSeconds)
                        .EnableRetryOnFailure(options.Value.MaxRetries)
                        .MigrationsHistoryTable(Tables.Dbo.EfMigrationsHistory, Schemas.Dbo)
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }
            )
            .UseEnumCheckConstraints()
            .UseExceptionProcessor()
            .UseSnakeCaseNamingConvention();
    }
}
