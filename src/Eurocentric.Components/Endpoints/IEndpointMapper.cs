using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Components.Endpoints;

/// <summary>
///     Defines the contract for a type that maps one or more endpoints.
/// </summary>
public interface IEndpointMapper
{
    /// <summary>
    ///     Maps one or more endpoints to the provided endpoint route builder.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    static abstract void MapTo(IEndpointRouteBuilder builder);
}
