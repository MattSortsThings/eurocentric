using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.OpenApi;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the OpenAPI documentation services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <param name="registrars">
    ///     A sequence of operations to be invoked on the application service descriptor collection, each
    ///     of which registers one or more OpenAPI documents.
    /// </param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddOpenApiDocumentation(
        this IServiceCollection services,
        params Action<IServiceCollection>[] registrars
    )
    {
        services.ConfigureOptions<ConfigureScalarOptions>();

        foreach (Action<IServiceCollection> registrar in registrars)
        {
            registrar(services);
        }

        return services;
    }
}
