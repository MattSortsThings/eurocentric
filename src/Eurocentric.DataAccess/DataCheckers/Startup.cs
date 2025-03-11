using Eurocentric.Domain.Rules.DataCheckers;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.DataAccess.DataCheckers;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Adds all the data checker services to the application services container.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddDataCheckers(this IServiceCollection services)
    {
        services.AddScoped<IDataChecker, DataChecker>();

        return services;
    }
}
