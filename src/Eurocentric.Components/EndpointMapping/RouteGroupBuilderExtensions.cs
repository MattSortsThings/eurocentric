using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Components.EndpointMapping;

/// <summary>
///     Extension methods for the <see cref="RouteGroupBuilder" /> class.
/// </summary>
public static class RouteGroupBuilderExtensions
{
    /// <summary>
    ///     Maps the specified endpoint to the endpoint group.
    /// </summary>
    /// <param name="routeGroupBuilder">The endpoint route group builder.</param>
    /// <typeparam name="T">The endpoint mapper type.</typeparam>
    /// <returns>The same <see cref="RouteGroupBuilder" /> instance, so that method invocations can be chained.</returns>
    public static RouteGroupBuilder Map<T>(this RouteGroupBuilder routeGroupBuilder)
        where T : IEndpointMapper, new()
    {
        T endpoint = new();

        endpoint.MapEndpoint(routeGroupBuilder);

        return routeGroupBuilder;
    }
}
