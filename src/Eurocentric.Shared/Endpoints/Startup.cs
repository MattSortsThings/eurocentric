using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.Endpoints;

/// <summary>
///     Extension methods to be invoked on application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Maps all the API endpoints that have been configured for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseApiEndpoints(this IEndpointRouteBuilder app)
    {
        foreach (IEndpointMapper mapper in GetEndpointMappers(app))
        {
            mapper.MapEndpoints(app);
        }
    }

    private static IEnumerable<IEndpointMapper> GetEndpointMappers(IEndpointRouteBuilder app)
    {
        using IServiceScope scope = app.ServiceProvider.CreateScope();

        return scope.ServiceProvider.GetRequiredService<IEnumerable<IEndpointMapper>>();
    }
}
