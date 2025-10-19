using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Components.EndpointMapping;

/// <summary>
///     A class that maps a single endpoint.
/// </summary>
public interface IEndpointMapper
{
    /// <summary>
    ///     Maps an endpoint to the route group builder.
    /// </summary>
    /// <param name="routeBuilder">The route group builder.</param>
    void MapEndpoint(RouteGroupBuilder routeBuilder);
}
