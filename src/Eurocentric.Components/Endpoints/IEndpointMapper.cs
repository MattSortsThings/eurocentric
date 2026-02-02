using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Components.Endpoints;

/// <summary>
///     Defines the contract for an object that maps one or more endpoints.
/// </summary>
public interface IEndpointMapper
{
    /// <summary>
    ///     Maps one or more endpoints to the route builder.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    void Map(IEndpointRouteBuilder builder);
}
