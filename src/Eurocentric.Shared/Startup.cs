using Eurocentric.Shared.ApiMapping;
using Eurocentric.Shared.AppPipeline;
using Eurocentric.Shared.Json;
using Eurocentric.Shared.Timing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the shared services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddAppPipeline()
            .AddJsonOptionsConfiguration()
            .AddTimeProvider();

        return services;
    }

    /// <summary>
    ///     Configures the web app to use the API endpoints that have been registered.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseApiEndpoints(this IEndpointRouteBuilder app)
    {
        using IServiceScope scope = app.ServiceProvider.CreateScope();

        foreach (IApiEndpointsMapper mapper in scope.ServiceProvider.GetRequiredService<IEnumerable<IApiEndpointsMapper>>())
        {
            mapper.Map(app);
        }
    }
}
