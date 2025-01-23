using Eurocentric.Shared.ApiModules;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.OpenApi;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Configures the OpenAPI documents for the application.
    /// </summary>
    /// <param name="services">The application service descriptor collection to be modified.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddOpenApiDocuments(this IServiceCollection services)
    {
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();

        foreach (IApiModule module in scope.ServiceProvider.GetRequiredService<IEnumerable<IApiModule>>())
        {
            module.AddOpenApiDocuments(services);
        }

        return services;
    }
}
