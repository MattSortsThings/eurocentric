using Eurocentric.Shared.AppPipeline;
using Eurocentric.Shared.ErrorHandling;
using Eurocentric.Shared.Json;
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
            .AddJsonConfiguration()
            .AddErrorHandling();

        return services;
    }

    public static IEndpointRouteBuilder UseApiEndpoints(this IEndpointRouteBuilder app)
    {
        using IServiceScope scope = app.ServiceProvider.CreateScope();

        Action<IEndpointRouteBuilder> mappingAction =
            scope.ServiceProvider.GetRequiredService<IEnumerable<Action<IEndpointRouteBuilder>>>()
                .Aggregate((IEndpointRouteBuilder _) => { },
                    (currentAction, action) => currentAction + action);

        mappingAction.Invoke(app);

        return app;
    }
}
