using Dapper;
using Eurocentric.Components.DataAccess.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.DataAccess.Dapper;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the Dapper data access services and configuration to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The original <see cref="IServiceCollection" /> instance.</returns>
    internal static IServiceCollection AddDapperDataAccess(this IServiceCollection services)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        services.AddScoped<DbSprocRunner>(static serviceProvider =>
            DbSprocRunner.Create(serviceProvider.GetRequiredService<IOptions<DbConnectionOptions>>())
        );

        return services;
    }
}
