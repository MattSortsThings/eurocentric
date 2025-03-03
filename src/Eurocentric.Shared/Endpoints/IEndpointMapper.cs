using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.Endpoints;

/// <summary>
///     Defines a method for mapping API endpoints.
/// </summary>
/// <remarks>Register an implementation of this interface as a transient service in each API project.</remarks>
public interface IEndpointMapper
{
    /// <summary>
    ///     Maps all the endpoints for the API.
    /// </summary>
    /// <param name="app">The web application.</param>
    public void MapEndpoints(IEndpointRouteBuilder app);
}
