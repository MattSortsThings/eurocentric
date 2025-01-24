using Eurocentric.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.DataAccess;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the data access services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">The application service descriptor collection to be modified.</param>
    /// <param name="configuration">Contains application configuration properties.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppDbContext(configuration.GetConnectionString(DatabaseConstants.ConnectionStringKey));

        return services;
    }

    /// <summary>
    ///     Adds the EF Core application DB context to the application service descriptor collection using the specified
    ///     connection string.
    /// </summary>
    /// <param name="services">The application service descriptor collection to be modified.</param>
    /// <param name="connectionString">The database connection string.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString, optionsBuilder =>
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });

        return services;
    }
}
