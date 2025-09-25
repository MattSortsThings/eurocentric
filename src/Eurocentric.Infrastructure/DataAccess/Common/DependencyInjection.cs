using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.DataAccess.Common;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the Azure SQL DB options to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddAzureSqlDbOptions(this IServiceCollection services)
    {
        services.ConfigureOptions<AzureSqlDbOptionsConfig>();

        return services;
    }
}
