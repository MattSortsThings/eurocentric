using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds all the services from the <see cref="Eurocentric.Infrastructure" /> assembly to the application service
    ///     descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services;
}
