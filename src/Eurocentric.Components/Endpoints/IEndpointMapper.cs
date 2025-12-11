using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Components.Endpoints;

/// <summary>
///     A class that maps one or more endpoints.
/// </summary>
public interface IEndpointMapper
{
    /// <summary>
    ///     Maps endpoints to the provided route builder.
    /// </summary>
    /// <param name="routeBuilder">The route builder.</param>
    void Map(IEndpointRouteBuilder routeBuilder);
}
