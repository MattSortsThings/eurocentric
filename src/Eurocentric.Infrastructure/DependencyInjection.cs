using Eurocentric.Infrastructure.DataAccess.Dapper;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Eurocentric.Infrastructure.IdProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the <see cref="Eurocentric.Infrastructure" /> assembly services to the application service descriptor
    ///     collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDapperDataAccess()
            .AddEfCoreDataAccess()
            .AddIdProviders();

        return services;
    }
}
